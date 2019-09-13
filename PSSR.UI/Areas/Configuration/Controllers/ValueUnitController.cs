
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.DataLayer.EfCode;
using PSSR.Logic.ValueUnits;
using BskaGenericCoreLib;
using PSSR.UI.Helpers;
using PSSR.ServiceLayer.ValueUnits;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.CashHelper;
using Microsoft.AspNetCore.Authorization;
using PSSR.UI.Helpers.Security;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using PSSR.UI.Helpers.Http;
using Microsoft.Extensions.Options;
using PSSR.UI.Configuration;
using PSSR.DataLayer.EfClasses.Management;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [ApiVersion("1.0")]
    public class ValueUnitController : BaseAdminController
    {
        private readonly EfCoreContext _context;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        private readonly IMasterDataCacheOperations _masterDataCache;
        public ValueUnitController(EfCoreContext context, IHttpClient clientService, IMasterDataCacheOperations masterDataCache
              , IOptions<ApplicationSettings> settings)
        {
            _context = context;
            _clientService = clientService;
            _masterDataCache = masterDataCache;
            _settings = settings;
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        public async Task<IActionResult> ValueUnit()
        {
            var viewModel = new ValueUnitListCombinedDto(null);

            await _masterDataCache.CreateMasterDataCacheAsync(User.GetCurrentUserDetails().Name, viewModel);
            return View(viewModel);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(List<ValueUnitListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetValueUnits()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetValueUnits",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateValueUnit(ValueUnitDto model,
           [FromServices]IActionService<IPlaceValuUnitAction> service)
        {
            var valueUnit = service.RunBizAction<ValueUnit>(model);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return RedirectToAction("ValueUnit");
            }

            service.Status.CopyErrorsToModelState(ModelState, model);

            SetupTraceInfo();       //Used to update the logs
            var viewModel = await _masterDataCache.GetMasterDataCacheAsync<ValueUnitListCombinedDto>(User.GetCurrentUserDetails().Name);
            return View("ValueUnit",viewModel);
        }
    }
}
