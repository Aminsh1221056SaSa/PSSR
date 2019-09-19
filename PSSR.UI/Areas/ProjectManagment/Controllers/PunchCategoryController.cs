
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BskaGenericCoreLib;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Web.Http;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DataLayer.EfCode;
using PSSR.Logic.PunchCategoryes;
using PSSR.ServiceLayer.Logger;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using PSSR.ServiceLayer.PunchCategoryServices;
using PSSR.ServiceLayer.PunchCategoryServices.Concrete;
using PSSR.ServiceLayer.Utils;
using PSSR.UI.Configuration;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers;
using PSSR.UI.Helpers.CashHelper;
using PSSR.UI.Helpers.Http;
using PSSR.UI.Helpers.Security;
using PSSR.UI.Models;

namespace PSSR.UI.Areas.ProjectManagment.Controllers
{
    [Area("ProjectManagment")]
    [ApiVersion("1.0")]
    public class PunchCategoryController : BaseManagerController
    {
        private readonly EfCoreContext _context;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        private readonly IMasterDataCacheOperations _masterDataCache;

        public PunchCategoryController(EfCoreContext context, IHttpClient clientService
            , IOptions<ApplicationSettings> settings, IMasterDataCacheOperations masterDataCache)
        {
            _context = context;
            _clientService = clientService;
            _settings = settings;
            _masterDataCache = masterDataCache;
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        public IActionResult PunchCategory(PunchCategorySortFilterPageOptions options)
        {
            var listService = new ListPunchCategoryService(_context);
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            var models= listService.SortFilterPage(options, project.Id).ToList();
            var viewModel = new PunchCategoryListCombinedDto(options,models);
            return View(viewModel);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(IEnumerable<PunchCategoryListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPunchCategoryes()
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerPunch/GetPunchCategoryes?projectId={project.Id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(PunchCategoryListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPunchCategory([FromQuery] int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerPunch/GetPunchCategory?id={id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        //
        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePunchCategory(PunchCategoryDto model,
         [FromServices]IActionService<IPlacePunchCategoryAction> service)
        {
            var projectService = new ListProjectService(_context);

            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            if (project != null)
            {
                model.ProjectId = project.Id;

                var pcate = service.RunBizAction<PunchCategory>(model);

                if (!service.Status.HasErrors)
                {
                    SetupTraceInfo();
                    return RedirectToAction("PunchCategory");
                }
                else
                {
                    service.Status.CopyErrorsToModelState(ModelState, model);
                }
            }

            service.Status.AddError("No find any project!", "Project");

            SetupTraceInfo();       //Used to update the logs
            return View("PunchCategory");
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePunchCategory(int pId, [FromServices]IActionService<IDeletePunchCategoryAction> service)
        {
            service.RunBizAction(pId);

            if (!service.Status.HasErrors)
            {
                return RedirectToAction("PunchCategory");
            }

            service.Status.CopyErrorsToModelState(ModelState, "Activity");
            return View("PunchCategory");
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPut]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        //[ValidateAntiForgeryToken]
        public IActionResult UpdatePunchCategory([FromBody] PunchCategoryDto model,
           [FromServices]IActionService<IUpdatePunchCategoryAction> service)
        {
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return new ObjectResult(new SuccessfullyResponseDto { Key = 200 });
            }

            service.Status.CopyErrorsToModelState(ModelState, model);

            SetupTraceInfo();       //Used to update the logs
            return BadRequest();
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        public JsonResult GetFilterSearchContent(PunchCategorySortFilterPageOptions options)
        {
            var service = new PunchCategoryFilterDropdownService(_context);

            var traceIdent = HttpContext.TraceIdentifier; //This makes the logging display work

            return Json(
                new TraceIndentGeneric<IEnumerable<DropdownTuple>>(
                traceIdent,
                service.GetFilterDropDownValues(
                    options.FilterBy)));
        }
    }
}