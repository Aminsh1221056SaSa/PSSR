
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Web.Http;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.MDRDocumentServices;
using PSSR.ServiceLayer.ProjectServices;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using PSSR.ServiceLayer.RoadMapServices.Concrete;
using PSSR.ServiceLayer.Utils.ChartsDto;
using PSSR.ServiceLayer.Utils.WorkPackageReportDto;
using PSSR.UI.Configuration;
using PSSR.UI.Helpers;
using PSSR.UI.Helpers.CashHelper;
using PSSR.UI.Helpers.Http;
using PSSR.UI.Helpers.Security;
using PSSR.UI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Controllers
{
    [ApiVersion("1.0")]
    public class DashboardController : BaseTraceController
    {
        private readonly EfCoreContext _context;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        private readonly IMasterDataCacheOperations _masterDataCache;

        public DashboardController(EfCoreContext context, IHttpClient clientService
            , IOptions<ApplicationSettings> settings, IMasterDataCacheOperations masterDataCache)
        {
            this._context = context;
            _clientService = clientService;
            _settings = settings;
            _masterDataCache = masterDataCache;
        }

        public IActionResult ProjectDashboard()
        {
            return View();
        }

        public IActionResult ManagerDashboard(Guid pid)
        {
            var user = User.GetCurrentUserDetails();
            _masterDataCache.SetUserCurrentProject(user.Name, pid);
            return View();
        }

        public IActionResult TaskDashboard()
        {
            return View();
        }

        public IActionResult ProjectWBS(Guid pid=default(Guid))
        {
            var user = User.GetCurrentUserDetails();
            _masterDataCache.SetUserCurrentProject(user.Name, pid);
            return View();
        }
        //render partial views
        public async Task<ActionResult> WorkPackageDetails(int wId,int locId)
        {
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var wbsService = new ListWBSService(_context,null);
            var viewModel = await wbsService.GetProjectWBSRelated(cpid,Common.WBSType.Location,wId,locId);

            return PartialView("~/Views/Shared/PartialViews/Reports/WorkPackageReport.cshtml", viewModel);
        }

        //api

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(IEnumerable<ProjectListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetallCurrentUserprojects()
        {
            var userdetails = User.GetCurrentUserDetails();
            if (userdetails.Roles.Any(s => s == "SuperAdministrator")
                || userdetails.Roles.Any(s => s == "Administrator"))
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Admin/GetProjectsForAdmin",
                    authorizationToken: accessToken);

                return new ObjectResult(content);
            }
            else
            {
                var actor = userdetails.Actor;
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetallCurrentUserprojects?personId=" +actor,
                    authorizationToken: accessToken);

                return new ObjectResult(content);
            }
        }

        [HttpGet("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(ProjectWBSListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectWBSTreeToProgress([FromQuery] bool toProgress)
        {
            var projectservice = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectservice.GetProject(cpid);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerWbs/GetProjectWBSTreeToProgress?toProgress={toProgress}&projectId={project.Id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }


        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(ProjectDashboardViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ProjectDashboardInitialization()
        {
            var projectservice = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectservice.GetProject(cpid);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerReport/ProjectDashboard?projectId={project.Id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(ProjectDashboardViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ManagerDashboardInitializationW1()
        {
            var projectservice = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectservice.GetProject(cpid);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerReport/ManagerDashboard?projectId={project.Id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        #region work Package

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(ManagerDashboardWorkPackageReport), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActivityDetailsByWorkPackage([FromQuery] int workPackageId, [FromQuery] int groupType)
        {
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerReport/GetActivityDetailsByWorkPackage?workPackageId={workPackageId}&groupType={groupType}&projectId={cpid}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }


        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(IEnumerable<WFReportList>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDesciplineWorkPackageReport(int wId)
        {
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerReport/GetDesciplineWorkPackageReport?workPackageId={wId}&projectId={cpid}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(IEnumerable<WFReportList>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSystemWorkPackageReport(int wId)
        {
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerReport/GetSystemWorkPackageReport?workPackageId={wId}&projectId={cpid}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(IEnumerable<WFReportList>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWorkPackageStepProgress(int wId)
        {
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerReport/GetWorkPackageStepProgress?workPackageId={wId}&projectId={cpid}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        #endregion

        #region mdrs

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(List<MDRSummaryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectMDRSummary()
        {
            var projectservice = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectservice.GetProject(cpid);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerMDR/GetProjectMDRSummary?projectId={project.Id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        #endregion

        #region charts

        //activity chatrs

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(BarChartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetGlobalActivityDoneChart()
        {
            var user = User.GetCurrentUserDetails();
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ChartReport/GetAllActivityTaskDonePerDayForUser?personId={user.Actor}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        //system charts

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(BarChartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskStatusBySystem()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ChartReport/TaskStatusBySystem",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(BarChartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskConditionBySystem()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ChartReport/TaskConditionBySystem",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(PieChartsListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskCounterBySystem()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ChartReport/TaskCounterBySystem",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(BarChartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskDoneBySystem()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ChartReport/TaskDoneBySystem",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        //descipline charts

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(BarChartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskStatusPreCommByDesciplines([FromQuery] int workPackageId, [FromQuery] int locationId, [FromQuery] bool total)
        {
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ChartReport/TaskStatusPreCommByDesciplines?workPackageId={workPackageId}&locationId={locationId}&total={total}&projectId={cpid}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(BarChartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskConditionByDesciplines()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ChartReport/TaskConditionByDesciplines",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(PieChartsListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskCounterByDesciplines()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ChartReport/TaskCounterByDesciplines",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(BarChartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskDoneByDesciplines()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ChartReport/TaskDoneByDesciplines",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        //workpackagestep charts

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(BarChartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskStatusPreCommByWorkStep([FromQuery] int workPackageId, [FromQuery] int locationId, [FromQuery] bool total)
        {
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ChartReport/TaskStatusPreCommByWorkStep?workPackageId={workPackageId}&locationId={locationId}&total={total}&projectId={cpid}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        #endregion
    }
}
