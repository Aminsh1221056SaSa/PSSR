
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.DataLayer.EfCode;
using System.Net;
using PSSR.ServiceLayer.RoadMapServices;
using PSSR.ServiceLayer.RoadMapServices.Concrete;
using PSSR.UI.Models;
using PSSR.Logic.RoadMaps;
using BskaGenericCoreLib;
using PSSR.UI.Helpers;
using PSSR.Logic.LocationTypes;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using PSSR.Logic.WorkPackages;
using PSSR.UI.Controllers;
using Microsoft.AspNetCore.Authentication;
using PSSR.UI.Helpers.Http;
using PSSR.UI.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using PSSR.DataLayer.EfClasses.Management;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [ApiVersion("1.0")]
    public class WorkPackageController : BaseAdminController
    {
        private readonly EfCoreContext _context;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;

        public WorkPackageController(EfCoreContext context, IHttpClient clientService
            ,IOptions<ApplicationSettings> settings)
        {
            _context = context;
            _clientService = clientService;
            _settings = settings;
        }

        // GET: /<controller>/
        public IActionResult RoadMap()
        {
            return View();
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(WorkPackageListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRoadMaps(string query)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetWorkPackages?query={query}",
                authorizationToken:accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(LocationListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLocations(string query)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetLocations?query={query}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }
        
        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(WorkPackageListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRoadMap([FromQuery] int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetWorkPackage?id={id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(LocationListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLocation([FromQuery] int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetLocation?id={id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        //
        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseModelDto<ProjectWorkPackageListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateRoadMap([FromBody] ProjectWorkPackageListDto model
            , [FromServices]IActionService<IPlaceWorkPackageAction> service)
        {
            var roadMapService = new ListWorkPackageService(_context);
            var roadMap = service.RunBizAction<WorkPackage>(model);

            if (!service.Status.HasErrors)
            {
                var query = await roadMapService.GetRoadMapsAsync();
                var backModel = query.LastOrDefault();

                SetupTraceInfo();
                return new ObjectResult(new SuccessfullyResponseModelDto<WorkPackageListDto>
                {
                    Model = backModel,
                    Key = 200,
                    Value = "Create Road map success!!"
                });
            }

            service.Status.CopyErrorsToModelState(ModelState, model);

            return new ObjectResult(new SuccessfullyResponseModelDto<WorkPackageListDto> {Model=null, Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateLocation([FromBody] LocationTypeDto model
           , [FromServices]IActionService<IPlaceLocationTypeAction> service)
        {
            var projectService = new ListProjectService(_context);
            var roadMapService = new ListWorkPackageService(_context);

            var location = service.RunBizAction<LocationType>(model);

            if (!service.Status.HasErrors)
            {
                var query = await roadMapService.GetLocationsAsync();
                var backModel = query.LastOrDefault();

                SetupTraceInfo();
                return new ObjectResult(new SuccessfullyResponseModelDto<LocationListDto> { Model = backModel, Key = 200, Value = "Create Location success!!" });
            }

            service.Status.CopyErrorsToModelState(ModelState, model);

            SetupTraceInfo();       //Used to update the logs
            return new ObjectResult(new SuccessfullyResponseModelDto<LocationListDto> {Model=null,Key = -1, Value = service.Status.GetAllErrors() });
        }


        //
        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPut]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult UpdateRoadMap([FromBody] ProjectWorkPackageListDto model
           , [FromServices]IActionService<IUpdateRoadMapAction> service)
        {
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return new ObjectResult(new SuccessfullyResponseDto { Key = 200, Value = "Update Road map success!!" });
            }

            service.Status.CopyErrorsToModelState(ModelState, model);

            SetupTraceInfo();       //Used to update the logs
            return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPut]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult UpdateLocation([FromBody] LocationTypeDto model
          , [FromServices]IActionService<IUpdateLocationTypeAction> service)
        {
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return new ObjectResult(new SuccessfullyResponseDto { Key = 200, Value = "Update location success!!" });
            }

            service.Status.CopyErrorsToModelState(ModelState, model);

            SetupTraceInfo();       //Used to update the logs
            return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpDelete("APSE/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult DeleteWorkPackage(int id
         , [FromServices]IActionService<IDeleteWorkPackageAction> service)
        {
            service.RunBizAction(id);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return new ObjectResult(new SuccessfullyResponseDto { Key = 200, Value = "delete workpackage success!!" });
            }

            service.Status.CopyErrorsToModelState(ModelState, id);

            SetupTraceInfo();       //Used to update the logs
            return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpDelete("APSE/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult DeleteLocation(int id, [FromServices]IActionService<ILocationDeleteAction> service)
        {
            service.RunBizAction(id);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return new ObjectResult(new SuccessfullyResponseDto { Key = 200, Value = "delete location success!!" });
            }

            service.Status.CopyErrorsToModelState(ModelState, id);

            SetupTraceInfo();       //Used to update the logs
            return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value = service.Status.GetAllErrors() });
        }
    }
}
