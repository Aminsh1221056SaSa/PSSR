using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.SubSystemServices.Concrete;
using PSSR.ServiceLayer.SubSystemServices;
using BskaGenericCoreLib;
using PSSR.Logic.ProjectSubSystems;
using PSSR.ServiceLayer.Utils;
using PSSR.ServiceLayer.Logger;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.UI.Helpers;
using PSSR.ServiceLayer.ProjectSystemServices.Concrete;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using System.Threading.Tasks;
using System.Net;
using PSSR.UI.Models;
using Microsoft.AspNetCore.Http;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.CashHelper;
using PSSR.UI.Helpers.Http;
using Microsoft.Extensions.Options;
using PSSR.UI.Configuration;
using Microsoft.AspNetCore.Authentication;
using PSSR.UI.Helpers.Security;
using PSSR.ServiceLayer.ProjectServices;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.ProjectManagment.Controllers
{
    [Area("ProjectManagment")]
    [ApiVersion("1.0")]
    public class SubSystemController : BaseManagerController
    {
        private readonly EfCoreContext _context;
        private readonly IMasterDataCacheOperations _masterDataCache;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;

        public SubSystemController(EfCoreContext context, IMasterDataCacheOperations masterDataCache, IHttpClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _context = context;
            _masterDataCache = masterDataCache;
            _clientService = clientService;
            _settings = settings;
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        // GET: /<controller>/
        public async Task<IActionResult> SubSystem(ProjectSubSystmeSortFilterPageOptions options)
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            var listService =new ListProjectSubSystemService(_context);
            
            var listSystemService = new ListProjectSystemService(_context);

            var systems =await listSystemService.GetProjectSystems(cpid);

            var projectSubSystemList =(await listService.SortFilterPage(options,project.Id)).ToList();
            var viewModel = new ProjectSubSustemListCombinedDto(options, projectSubSystemList, systems);

            await _masterDataCache.CreateMasterDataCacheAsync(User.GetCurrentUserDetails().Name, viewModel);
            SetupTraceInfo();           //Thsi makes the logging display work
            return View(viewModel);
        }
       
        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(ProjectSubSystemListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSubSystem([FromQuery] int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetSubSystem?id={id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(ProjectMapDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectSubSystems()
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetProjectSubSystems?projectId={project.Id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(ProjectSubSystemListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSubSystemBySystem([FromQuery] int systemId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetSubSystemBySystem?systemId={systemId}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        //
        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProjectSubSystem(ProjectSubSystemDto model,
          [FromServices]IActionService<IPlaceSubSystemAction> service)
        {
            var descipline = service.RunBizAction<ProjectSubSystem>(model);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return RedirectToAction("SubSystem");
            }

            service.Status.CopyErrorsToModelState(ModelState, model);
            var viewModel = await _masterDataCache.GetMasterDataCacheAsync<ProjectSubSustemListCombinedDto>(User.GetCurrentUserDetails().Name);
            SetupTraceInfo();       //Used to update the logs
            return View("SubSystem", viewModel);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UploadExcel(IFormFile file,
       [FromServices]IActionService<IPlcaeSubSystemBulkAction> service)
        {
            if (file != null)
            {
                ExcelFileConverterHelper converter = new ExcelFileConverterHelper();
                var projectService = new ListProjectService(_context);

                var user = User.GetCurrentUserDetails();
                var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
                var project = projectService.GetProject(cpid);
                if (project == null)
                {
                    service.Status.AddError("Project Not Found!!!", "sub system");
                }
                var listSystemService =new ListProjectSystemService(_context);

                var systems = await listSystemService.GetProjectSystems(project.Id);
                if (!systems.Any())
                {
                    service.Status.AddError("Not available systems!!!", "sub system");
                }

                if (!service.Status.HasErrors)
                {
                    var systemdic = systems.ToDictionary(x => x.Title.ToUpper(), x =>Convert.ToInt32(x.Id));
                    var model = await converter.ParseSubSystemExcel(file, systemdic, project.Id);
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
                service.Status.AddError("please select a file", "System");
            }

            SetupTraceInfo();       //Used to update the logs
            return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPut]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult UpdateProjectSubSystem([FromBody] ProjectSubSystemDto model,
           [FromServices]IActionService<IUpdateSubSystemAction> service)
        {
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return new ObjectResult(new SuccessfullyResponseDto { Key = 200, Value = "subsystem success to update" });
            }

            service.Status.CopyErrorsToModelState(ModelState, model);


            SetupTraceInfo();       //Used to update the logs
            return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        public JsonResult GetFilterSearchContent(ProjectSubSystmeSortFilterPageOptions options)
        {
            var service = new ProjectSubSystemFilterDropdownService(_context);

            var traceIdent = HttpContext.TraceIdentifier; //This makes the logging display work

            return Json(
                new TraceIndentGeneric<IEnumerable<DropdownTuple>>(
                traceIdent,
                service.GetFilterDropDownValues(
                    options.FilterBy)));
        }
    }
}
