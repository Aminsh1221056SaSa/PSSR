using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ProjectSystemServices.Concrete;
using PSSR.ServiceLayer.ProjectSystemServices;
using BskaGenericCoreLib;
using PSSR.Logic.ProjectSystmes;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.UI.Helpers;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using Microsoft.Web.Http;
using PSSR.ServiceLayer.Logger;
using PSSR.ServiceLayer.Utils;
using PSSR.UI.Models;
using System.Net;
using Microsoft.AspNetCore.Http;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.CashHelper;
using PSSR.UI.Configuration;
using Microsoft.Extensions.Options;
using PSSR.UI.Helpers.Http;
using Microsoft.AspNetCore.Authentication;
using PSSR.UI.Helpers.Security;
using PSSR.ServiceLayer.ProjectServices;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.ProjectManagment.Controllers
{
    [Area("ProjectManagment")]
    [ApiVersion("1.0")]
    public class ProjectSystemController : BaseManagerController
    {

        private readonly EfCoreContext _context;
        private readonly IMasterDataCacheOperations _masterDataCache;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;

        public ProjectSystemController(EfCoreContext context, IMasterDataCacheOperations masterDataCache, IHttpClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _context = context;
            _masterDataCache = masterDataCache;
            _clientService = clientService;
            _settings = settings;
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        // GET: /<controller>/S
        public async Task<IActionResult> ProjectSystem(ProjectSystmeSortFilterPageOptions options)
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            var listService = new ListProjectSystemService(_context);

            var projectSystemList = listService.SortFilterPage(options,project.Id).ToList();
            var viewModel = new ProjectSystemListCombinedDto(options, projectSystemList);
            await _masterDataCache.CreateMasterDataCacheAsync(User.GetCurrentUserDetails().Name, viewModel);
            SetupTraceInfo();           //Thsi makes the logging display work
            return View(viewModel);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProjectSystem(ProjectSystemDto model,
           [FromServices]IActionService<IPlaceSystemAction> service)
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);
            model.ProjectId = project.Id;

            var descipline = service.RunBizAction<ProjectSystem>(model);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return RedirectToAction("ProjectSystem");
            }

            service.Status.CopyErrorsToModelState(ModelState, model);

            var viewModel = await _masterDataCache.GetMasterDataCacheAsync<ProjectSystemListCombinedDto>(User.GetCurrentUserDetails().Name);
            SetupTraceInfo();       //Used to update the logs
            return View("ProjectSystem",viewModel);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(ProjectSystemListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectSystem([FromQuery] int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetProjectSystemDirect?systemId={id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(ProjectMapDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectSystems()
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetProjectSystems?projectId={project.Id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        //
        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPut]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult UpdateProjectSystem([FromBody] ProjectSystemDto model,
           [FromServices]IActionService<IUpdateSystemAction> service)
        {
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return  new ObjectResult(new SuccessfullyResponseDto { Key=200,Value="system success to update"});
            }

            service.Status.CopyErrorsToModelState(ModelState, model);


            SetupTraceInfo();       //Used to update the logs
            return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UploadExcel(IFormFile file,
         [FromServices]IActionService<IPlcaeSystemBulkAction> service)
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
                    service.Status.AddError("Project Not Found!!!", "Form Dictionary");
                }

                if (!service.Status.HasErrors)
                {
                    var model = await converter.ParseSystemExcel(file, project.Id);
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
        [HttpGet]
        public JsonResult GetFilterSearchContent(ProjectSystmeSortFilterPageOptions options)
        {
            var service = new ProjectSystemFilterDropdownService(_context);

            var traceIdent = HttpContext.TraceIdentifier; //This makes the logging display work

            return Json(
                new TraceIndentGeneric<IEnumerable<DropdownTuple>>(
                traceIdent,
                service.GetFilterDropDownValues(
                    options.FilterBy)));
        }
    }
}
