
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ProjectServices;
using System.Net;
using Microsoft.Web.Http;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.Http;
using Microsoft.Extensions.Options;
using PSSR.UI.Configuration;
using Microsoft.AspNetCore.Authentication;
using PSSR.UI.Helpers.CashHelper;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using PSSR.UI.Helpers.Security;
using System;
using PSSR.ServiceLayer.ActivityServices;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.ProjectManagment.Controllers
{
    [Area("ProjectManagment")]
    [ApiVersion("1.0")]
    public class ProjectController : BaseManagerController
    {
        private readonly EfCoreContext _context;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        private readonly IMasterDataCacheOperations _masterDataCache;

        public ProjectController(EfCoreContext context, IHttpClient clientService
            ,IOptions<ApplicationSettings> settings, IMasterDataCacheOperations masterDataCache)
        {
            _context = context;
            _clientService = clientService;
            _settings = settings;
            _masterDataCache = masterDataCache;
        }

        public IActionResult ProjectData()
        {
            return View();
        }

        public IActionResult ProjectWBSCreator()
        {
            return View();
        }
        
        public IActionResult DesciplinePlane(Guid pid = default(Guid))
        {
            var user = User.GetCurrentUserDetails();
            _masterDataCache.SetUserCurrentProject(user.Name, pid);
            return View();
        }

        public IActionResult PlanWorkFlow()
        {
            return View();
        }
        //
       
        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(IEnumerable<DesciplinePlanModelDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDesciplineActivityGroupList([FromQuery] int workPackageId, [FromQuery] int locationId, [FromQuery] int systemId, 
            [FromQuery] int subsystemId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetDesciplineActivityGroupList?workPackageId={workPackageId}&locationId={locationId}&systemId={systemId}&subsystemId={subsystemId}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(FormDicPlanModelDto), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> GetFormDictionaryGroupedByPlanDate(int workId, int locationId, int subSystemId, int desId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetFormDictionaryGroupedByPlanDate?workId={workId}&locationId={locationId}&subSystemId={subSystemId}&desId={desId}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(IEnumerable<PlanActivityDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectPlanActivity(int workPackageId, int locationId, long subsystemId, int desciplineId,long formid)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetProjectPlanActivity?workPackageId={workPackageId}&locationId={locationId}&subsystemId={subsystemId}&desciplineId={desciplineId}&formid={formid}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(IEnumerable<HirecharyPlaneDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPlanHirechary([FromQuery]string filterTypes)
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetPlanHirechary?filterTypes={filterTypes}&projectId={project.Id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

    }
}
