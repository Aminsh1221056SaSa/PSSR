
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.PunchTypeServices.Concrete;
using PSSR.ServiceLayer.PunchTypeServices;
using BskaGenericCoreLib;
using PSSR.Logic.PunchTypes;
using PSSR.UI.Helpers;
using System.Net;
using PSSR.UI.Models;
using PSSR.ServiceLayer.Logger;
using PSSR.ServiceLayer.Utils;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using PSSR.ServiceLayer.RoadMapServices.Concrete;
using Newtonsoft.Json;
using PSSR.UI.Controllers;
using Microsoft.Extensions.Options;
using PSSR.UI.Helpers.Http;
using PSSR.UI.Configuration;
using Microsoft.AspNetCore.Authentication;
using PSSR.UI.Helpers.Security;
using PSSR.UI.Helpers.CashHelper;
using Microsoft.AspNetCore.Authorization;
using PSSR.DataLayer.EfClasses.Projects.Activities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.ProjectManagment.Controllers
{
    [Area("ProjectManagment")]
    [ApiVersion("1.0")]
    public class PunchTypeController : BaseManagerController
    {
        private readonly EfCoreContext _context;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        private readonly IMasterDataCacheOperations _masterDataCache;

        public PunchTypeController(EfCoreContext context, IHttpClient clientService
            , IOptions<ApplicationSettings> settings, IMasterDataCacheOperations masterDataCache)
        {
            _context = context;
            _clientService = clientService;
            _settings = settings;
            _masterDataCache = masterDataCache;
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        public async Task<IActionResult> PunchType(PunchTypeSortFilterPageOptions options)
        {
            var listService =new ListPunchTypeService(_context);
            var roadMapService = new ListWorkPackageService(_context);
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            var punchTypeList =await listService.GetPunchTypes(project.Id);

            ViewBag.WorkPackages = await roadMapService.GetRoadMapsAsync();
            SetupTraceInfo();           //Thsi makes the logging display work
            return View(punchTypeList);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(PunchTypeListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllPunchTypes()
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerPunch/GetAllPunchTypes?projectId={project.Id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(PunchTypeListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPunchTypes([FromQuery] int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerPunch/GetPunchTypes?id={id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        //
        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePunchType(PunchTypeDto model,string WorkPackageDic,
           [FromServices]IActionService<IPlacePunchTypeAction> service)
        {
            if(string.IsNullOrWhiteSpace(WorkPackageDic))
            {
                service.Status.AddError("Work package precentage is required!", "Project");
                return View("PunchType");
            }

            var rItems = JsonConvert.DeserializeObject<List<KeyValuePair<int, float>>>(WorkPackageDic);
            model.WorkPackagepr=rItems.ToDictionary(x => x.Key, x => x.Value);

            var projectService = new ListProjectService(_context);

            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            if (project != null)
            {
                model.ProjectId = project.Id;

                var descipline = service.RunBizAction<PunchType>(model);

                if (!service.Status.HasErrors)
                {
                    SetupTraceInfo();
                    return RedirectToAction("PunchType");
                }
                else
                {
                    service.Status.CopyErrorsToModelState(ModelState, model);
                }
            }

            service.Status.AddError("No find any project!", "Project");

            SetupTraceInfo();       //Used to update the logs
            return View("PunchType");
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePunchType(int pId,[FromServices]IActionService<IDeletePunchTypeAction> service)
        {
            service.RunBizAction(pId);

            if (!service.Status.HasErrors)
            {
                return RedirectToAction("PunchType");
            }

            service.Status.CopyErrorsToModelState(ModelState, "Activity");
            return View("PunchType");
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        //[ValidateAntiForgeryToken]
        public IActionResult UpdatePunchType([FromBody] PunchTypeDto model,
           [FromServices]IActionService<IUpdatePunchTypeAction> service)
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
        public JsonResult GetFilterSearchContent(PunchTypeSortFilterPageOptions options)
        {
            var service = new PunchTypeFilterDropdownService(_context);

            var traceIdent = HttpContext.TraceIdentifier; //This makes the logging display work

            return Json(
                new TraceIndentGeneric<IEnumerable<DropdownTuple>>(
                traceIdent,
                service.GetFilterDropDownValues(
                    options.FilterBy)));
        }
    }
}
