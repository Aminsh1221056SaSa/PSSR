using BskaGenericCoreLib;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Web.Http;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfCode;
using PSSR.Logic.WorkPackageSteps;
using PSSR.ServiceLayer.Logger;
using PSSR.ServiceLayer.RoadMapServices.Concrete;
using PSSR.ServiceLayer.Utils;
using PSSR.ServiceLayer.WorkPackageSteps;
using PSSR.ServiceLayer.WorkPackageSteps.Concrete;
using PSSR.UI.Configuration;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers;
using PSSR.UI.Helpers.CashHelper;
using PSSR.UI.Helpers.Http;
using PSSR.UI.Helpers.Security;
using PSSR.UI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PSSR.UI.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [ApiVersion("1.0")]
    public class WorkPackageStepController: BaseAdminController
    {
        private readonly EfCoreContext _context;
        private readonly IMasterDataCacheOperations _masterDataCache;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        public WorkPackageStepController(EfCoreContext context, IMasterDataCacheOperations masterDataCache,
            IHttpClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _context = context;
            _masterDataCache = masterDataCache;
            _clientService = clientService;
            _settings = settings;
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        public async Task<IActionResult> WorkPackageStep(WorkPackageStepSortFilterPageOptions options)
        {
            var listService = new WorkPackageStepService(_context);
            var workPackageService = new ListWorkPackageService(_context);

            var activityList= await Task.Run(() =>
            {
              return  listService
                .SortFilterPage(options).ToList();
            });
            ViewBag.WorkPackages = await workPackageService.GetRoadMapsAsync();
            SetupTraceInfo();           //Thsi makes the logging display work
            var viewModel = new WorkPackageStepListCombinedDto(options, activityList);

            await _masterDataCache.CreateMasterDataCacheAsync(User.GetCurrentUserDetails().Name, viewModel);
            return View(viewModel);
        }
        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(WorkPackageStepListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWorkPackageStep([FromQuery] int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetWorkPackageStep?id={id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(List<WorkPackageStepListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWorkPackageSteps()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetWorkPackageSteps",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(WorkPackageStepListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWorkPackageStepByWorkPackage(int wid)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetWorkPackageStepByWorkPackage?workPackageId={wid}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        //
        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWorkPackageStep(WorkPackageStepDto model,
          [FromServices]IActionService<IPlaceWorkStepPackageAction> service)
        {
            var wStep = service.RunBizAction<WorkPackageStep>(model);
            if (!service.Status.HasErrors)
            {
                return RedirectToAction("WorkPackageStep");
            }

            service.Status.CopyErrorsToModelState(ModelState, model);

            SetupTraceInfo();       //Used to update the logs

            var viewModel = await _masterDataCache.GetMasterDataCacheAsync<WorkPackageStepListCombinedDto>(User.GetCurrentUserDetails().Name);
            return View("WorkPackageStep", viewModel);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult UpdateWorkPackageStep([FromBody] WorkPackageStepDto model,
            [FromServices]IActionService<IUpdateWorkPackageStepAction> service)
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
        public JsonResult GetFilterSearchContent(WorkPackageStepSortFilterPageOptions options)
        {
            var service = new WorkPackageStepFilterDropdownService(_context);

            var traceIdent = HttpContext.TraceIdentifier; //This makes the logging display work

            return Json(
                new TraceIndentGeneric<IEnumerable<DropdownTuple>>(
                traceIdent,
                service.GetFilterDropDownValues(options.FilterBy)));
        }
    }
}
