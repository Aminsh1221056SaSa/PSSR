
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ProjectServices;
using PSSR.Logic.Projects;
using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.UI.Helpers;
using System.Net;
using Microsoft.Web.Http;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.Http;
using Microsoft.Extensions.Options;
using PSSR.UI.Configuration;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using PSSR.UI.Helpers.CashHelper;
using PSSR.ServiceLayer.Logger;
using PSSR.ServiceLayer.Utils;
using PSSR.ServiceLayer.ContractorServices.Concrete;
using System;
using PSSR.UI.Helpers.Security;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiVersion("1.0")]
    public class ProjectController : BaseAdminController
    {
        private readonly EfCoreContext _context;
        private readonly IMasterDataCacheOperations _masterDataCache;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        public ProjectController(EfCoreContext context, IHttpClient clientService
            ,IOptions<ApplicationSettings> settings, IMasterDataCacheOperations masterDataCache)
        {
            _context = context;
            _clientService = clientService;
            _masterDataCache = masterDataCache;
            _settings = settings;
        }

        [Authorize(Policy = "dataEventRecordsAdmin")]
        // GET: /<controller>/
        public async Task<IActionResult> Project(ProjectSortFilterPageOptions options)
        {
            var projectService = new ListProjectService(_context);
            var contractorService = new ListContractorService(_context);
            var contractors =await contractorService.GetAllContractors();

            var items = await Task.Run(() =>
            {
                return projectService.SortFilterPage(options).ToList();
            });

            SetupTraceInfo();           //Thsi makes the logging display work
            var viewModel = new ProjectListCombinedDto(options, items, contractors);
            await _masterDataCache.CreateMasterDataCacheAsync(User.GetCurrentUserDetails().Name, viewModel);
            return View(viewModel);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(ProjectListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProject([FromQuery]Guid projectId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Admin/GetProject?projectId={projectId}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        //
        [Authorize(Policy = "dataEventRecordsAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProject(ProjectDto model,
           [FromServices]IActionService<IPlaceProjectAction> service)
        {
            var project = service.RunBizAction<Project>(model);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return RedirectToAction("Project");
            }

            service.Status.CopyErrorsToModelState(ModelState, model);
            var viewModel = await _masterDataCache.GetMasterDataCacheAsync<ProjectSortFilterPageOptions>(User.GetCurrentUserDetails().Name);

            SetupTraceInfo();       //Used to update the logs
            return View("Project", viewModel);
        }

        [Authorize(Policy = "dataEventRecordsAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProject(ProjectDto model,
          [FromServices]IActionService<IUpdateProjectAction> service)
        {
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return RedirectToAction("Project");
            }

            service.Status.CopyErrorsToModelState(ModelState, model);
            var viewModel = await _masterDataCache.GetMasterDataCacheAsync<ProjectSortFilterPageOptions>(User.GetCurrentUserDetails().Name);
            SetupTraceInfo();       //Used to update the logs
            return View("Project", viewModel);
        }

        //
        [Authorize(Policy = "dataEventRecordsAdmin")]
        [HttpGet]
        public JsonResult GetFilterSearchContent(ProjectSortFilterPageOptions options)
        {
            var service = new ProjectFilterDropdownService(_context);

            var traceIdent = HttpContext.TraceIdentifier;

            return Json(new TraceIndentGeneric<IEnumerable<DropdownTuple>>(traceIdent,service
                .GetFilterDropDownValues(options.FilterBy)));
        }
    }
}
