using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.FormDictionaryServices.Concrete;
using PSSR.ServiceLayer.FormDictionaryServices;
using PSSR.ServiceLayer.DesciplineServices.Concrete;
using PSSR.Logic.FormDictionaries;
using BskaGenericCoreLib;
using PSSR.UI.Helpers;
using System.Net;
using PSSR.UI.Models;
using PSSR.ServiceLayer.Logger;
using PSSR.ServiceLayer.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PSSR.ServiceLayer.RoadMapServices.Concrete;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using System.IO;
using PSSR.UI.Helpers.CashHelper;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.Http;
using Microsoft.Extensions.Options;
using PSSR.UI.Configuration;
using Microsoft.AspNetCore.Authentication;
using PSSR.UI.Helpers.Security;
using Microsoft.AspNetCore.Authorization;
using PSSR.DataLayer.EfClasses.Management;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [ApiVersion("1.0")]
    public class FormDictionaryController : BaseAdminController
    {
        private readonly IHostingEnvironment _enviroment;
        private readonly IMasterDataCacheOperations _masterDataCache;
        private readonly EfCoreContext _context;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;

        public FormDictionaryController(EfCoreContext context, IHostingEnvironment enviroment,
            IMasterDataCacheOperations masterDataCache, IHttpClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _context = context;
            this._enviroment = enviroment;
            _masterDataCache = masterDataCache;
            _clientService = clientService;
            _settings = settings;
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        public async Task<IActionResult> FormDictionary(FormDictionarySortFilterPageOptions options)
        {
            var desciplineList =new ListDesciplineService(_context);
            var roadMapService = new ListWorkPackageService(_context);

            var listService =new ListFormDictionaryService(_context);

            var desciplines =await desciplineList
                .GetAllDesciplines();

            var formDicList = listService
                .SortFilterPage(options)
                .ToList();
            var workPackages =await roadMapService.GetRoadMapsAsync();
            SetupTraceInfo();           //Thsi makes the logging display work

            var viewModel = new FormDictionaryListCombinedDto(options, formDicList, desciplines, workPackages);

            await _masterDataCache.CreateMasterDataCacheAsync(User.GetCurrentUserDetails().Name, viewModel);

            return View(viewModel);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(FormDictionaryListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFormDictionary([FromQuery] long id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetFormDictionary?id={id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(List<FormDictionarySummaryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetformDictionaryies()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetformDictionaryies",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(ServiceLayer.DesciplineServices.DesciplineListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFormsByDescipline([FromQuery] int desciplineId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetFormsByDescipline?desciplineId={desciplineId}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        public async Task<IActionResult> DowanloadExcelFile(int id)
        {
            var listService = new ListFormDictionaryService(_context);
            var result = await listService.GetformDictionary(id);
            var contentType = "APPLICATION/vnd.ms-exce";
            var formDocPath = Path.Combine($"{_enviroment.ContentRootPath}/wwwroot/exceldocuments");
            var fromPathXls = Path.Combine(formDocPath, $"{result.Code}.xls");

            if (!System.IO.File.Exists(fromPathXls))
            {
                fromPathXls = Path.Combine(formDocPath, $"{result.Code}.xlsx");
                if (!System.IO.File.Exists(fromPathXls))
                {
                    return null;
                }
            }

            var fileName = fromPathXls;
            byte[] fileBytes = System.IO.File.ReadAllBytes(fileName);
            return File(fileBytes, contentType, fileName);
        }

        //

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFormDictionary(FormDictionaryDto model,
           [FromServices]IActionService<IPlaceFormDictionaryAction> service)
        {
            if (model.File == null)
            {
                service.Status.AddError("File Not Valid!!!", "Form Dictionary");
            }

            var formService = new ListFormDictionaryService(_context);
            if (await formService.HasDuplicatedCode(model.Code))
            {
                service.Status.AddError("Entered code is taked from other form!!!", "Form Dictionary");
            }

            if (!service.Status.HasErrors)
            {
                if (model.File != null && model.File.Length > 0)
                {
                    FormDocumentFileHelper docHelper = new FormDocumentFileHelper();
                    string filePath = await docHelper.SaveFormDocument(model.Code, model.File, _enviroment);
                    model.FileName = Path.Combine(Path.Combine(filePath, $"{model.Code}"));
                }

                var formDic = service.RunBizAction<FormDictionary>(model);

                SetupTraceInfo();
                return RedirectToAction("FormDictionary");
            }

            var viewModel = await _masterDataCache.GetMasterDataCacheAsync<FormDictionaryListCombinedDto>(User.GetCurrentUserDetails().Name);
            service.Status.CopyErrorsToModelState(ModelState, model);

            SetupTraceInfo();       //Used to update the logs

            return View("FormDictionary", viewModel);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFormDictionary(FormDictionaryDto model,
           [FromServices]IActionService<IUpdateFormDictionaryAction> service)
        {
            if (model.File != null && model.File.Length > 0)
            {
                FormDocumentFileHelper docHelper = new FormDocumentFileHelper();
                string filePath = await docHelper.SaveFormDocument(model.Code, model.File, _enviroment);
                model.FileName = Path.Combine(Path.Combine(filePath, $"{model.Code}"));
            }

            service.RunBizAction(model);

            var viewModel = await _masterDataCache.GetMasterDataCacheAsync<FormDictionaryListCombinedDto>(User.GetCurrentUserDetails().Name);
            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return RedirectToAction("FormDictionary", viewModel.SortFilterPageData);
            }

            service.Status.CopyErrorsToModelState(ModelState, model);

            SetupTraceInfo();       //Used to update the logs
            return View("FormDictionary", viewModel);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UploadExcel(IFormFile file,
           [FromServices]IActionService<IPlcaeFormDictionaryBulkAction> service)
        {
            if (file != null)
            {
                ExcelFileConverterHelper converter = new ExcelFileConverterHelper();
                var roadMapService = new ListWorkPackageService(_context);
                var desciplineList = new ListDesciplineService(_context);
                var projectService = new ListProjectService(_context);

                var workPackages = await roadMapService.GetRoadMapsAsync();
                if (!workPackages.Any())
                {
                    service.Status.AddError("Not available WorkPackage!!!", "Form Dictionary");
                }
                var desciplines = await desciplineList.GetAllDesciplines();
                if (!desciplines.Any())
                {
                    service.Status.AddError("Not available desciplines!!!", "Form Dictionary");
                }

                if (!service.Status.HasErrors)
                {
                    var Desdic = desciplines.DistinctBy(x => x.Title).ToDictionary(x => x.Title.ToUpper(), x => x.Id);
                    var workDic = workPackages.DistinctBy(x => x.Title).ToDictionary(x => x.Title.ToUpper(), x => x.Id);

                    var model = await converter.ParseFormDictionaryExcel(file, Desdic, workDic);
                    if (string.IsNullOrWhiteSpace(model.Item1))
                    {

                        service.RunBizAction(model.Item2);

                        if (!service.Status.HasErrors)
                        {
                            SetupTraceInfo();
                            return new ObjectResult(new SuccessfullyResponseDto { Key = 200 });
                        }
                    }
                    else
                    {
                        service.Status.AddError(model.Item1);
                    }
                }
            }
            else
            {
                service.Status.AddError("please select a file", "Form Dictionary");
            }

            SetupTraceInfo();       //Used to update the logs
            return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        public JsonResult GetFilterSearchContent(FormDictionarySortFilterPageOptions options)
        {
            var service = new FormDictionaryFilterDropdownService(_context);

            var traceIdent = HttpContext.TraceIdentifier; //This makes the logging display work

            return Json(
                new TraceIndentGeneric<IEnumerable<DropdownTuple>>(
                traceIdent,
                service.GetFilterDropDownValues(
                    options.FilterBy)));
        }

    }
}
