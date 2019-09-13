using System;
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
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfClasses.Projects.MDRS;
using PSSR.DataLayer.EfCode;
using PSSR.Logic.MDRStatuses;
using PSSR.ServiceLayer.MDRStatuses;
using PSSR.ServiceLayer.MDRStatuses.Concrete;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using PSSR.UI.Configuration;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers;
using PSSR.UI.Helpers.CashHelper;
using PSSR.UI.Helpers.Http;
using PSSR.UI.Helpers.Security;
using PSSR.UI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.ProjectManagment.Controllers
{
    [Area("ProjectManagment")]
    [ApiVersion("1.0")]
    public class MDRConfigController : BaseManagerController
    {
        private readonly EfCoreContext _context;
        private readonly IMasterDataCacheOperations _masterDataCache;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        public MDRConfigController(EfCoreContext context, IMasterDataCacheOperations masterDataCache
            , IHttpClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _context = context;
            _masterDataCache = masterDataCache;
            _clientService = clientService;
            _settings = settings;
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        // GET: /<controller>/
        public  async Task<IActionResult> MDRStatusConfig(MDRStatusSortFilterPageOptions options)
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            var listService =new ListMDRStatusService(_context);

            var mdrStatusList = (await listService
                .SortFilterPage(options,project.Id)).ToList();

            SetupTraceInfo();           //Thsi makes the logging display work
            var viewModel = new MDRStatusListCombinedDto(options, mdrStatusList);
            ;
            await _masterDataCache.CreateMasterDataCacheAsync(User.GetCurrentUserDetails().Name, viewModel);
            return View(viewModel);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(MDRStatusListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMDRStatus([FromQuery] int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetMDRStatus?id={id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        //
        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMDRStatus(MDRStatusDto model,
            [FromServices]IActionService<IPlaceMDRStatusAction> service)
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            model.ProjectId = project.Id;

            var descipline = service.RunBizAction<MDRStatus>(model);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return RedirectToAction("MDRStatusConfig");
            }

            service.Status.CopyErrorsToModelState(ModelState, model);

            SetupTraceInfo();       //Used to update the logs
            var viewModel = await _masterDataCache.GetMasterDataCacheAsync<MDRStatusListCombinedDto>(User.GetCurrentUserDetails().Name);
            return View("MDRStatusConfig", viewModel);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMDRStatus(MDRStatusDto model,
           [FromServices]IActionService<IUpdateMDRStatusAction> service)
        {
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return RedirectToAction("MDRStatusConfig");
            }

            service.Status.CopyErrorsToModelState(ModelState, model);

            SetupTraceInfo();       //Used to update the logs
            var viewModel = await _masterDataCache.GetMasterDataCacheAsync<MDRStatusListCombinedDto>(User.GetCurrentUserDetails().Name);
            return View("MDRStatusConfig", viewModel);
        }
    }
}
