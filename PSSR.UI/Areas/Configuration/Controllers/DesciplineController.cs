using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using System.Net;

using BskaGenericCoreLib;
using PSSR.Logic.Desciplines;
using PSSR.UI.Models;
using PSSR.UI.Helpers;
using PSSR.ServiceLayer.DesciplineServices;
using PSSR.ServiceLayer.DesciplineServices.Concrete;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.Logger;
using PSSR.ServiceLayer.Utils;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.CashHelper;
using PSSR.UI.Helpers.Http;
using Microsoft.Extensions.Options;
using PSSR.UI.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using PSSR.UI.Helpers.Security;
using PSSR.DataLayer.EfClasses.Management;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [ApiVersion("1.0")]
    public class DesciplineController : BaseAdminController
    {
        private readonly EfCoreContext _context;
        private readonly IMasterDataCacheOperations _masterDataCache;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        public DesciplineController(EfCoreContext context, IMasterDataCacheOperations masterDataCache
            , IHttpClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _context = context;
            _masterDataCache = masterDataCache;
            _clientService = clientService;
            _settings = settings;
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        public async Task<IActionResult> Descipline(DesciplineSortFilterPageOptions options)
        {
            var listService =
                 new ListDesciplineService(_context);

            var desciplineList = listService
                .SortFilterPage(options)
                .ToList();

            SetupTraceInfo();           //Thsi makes the logging display work
            var viewModel = new DesciplineListCombinedDto(options, desciplineList);
            await _masterDataCache.CreateMasterDataCacheAsync(User.GetCurrentUserDetails().Name, viewModel);
            return View(viewModel);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDescipline(DesciplineDto model,
            [FromServices]IActionService<IPlaceDesciplineAction> service)
        {
            var dto = new PlaceDesciplineDto(model);
            var descipline = service.RunBizAction<Descipline>(dto);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return RedirectToAction("Descipline");
            }

            service.Status.CopyErrorsToModelState(ModelState, dto);

            SetupTraceInfo();       //Used to update the logs
            var viewModel = await _masterDataCache.GetMasterDataCacheAsync<DesciplineListCombinedDto>(User.GetCurrentUserDetails().Name);
            return View("Descipline", viewModel);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(DesciplineListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDescipline([FromQuery] int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetDescipline?id={id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(DesciplineListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDesciplineList()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerProject/GetDesciplineList",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult UpdateDescipline([FromBody] DesciplineDto model,
            [FromServices]IActionService<IUpdateDesciplineAction> service)
        {
            var dto = new PlaceDesciplineDto(model);
          
            service.RunBizAction(dto);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return new ObjectResult(new SuccessfullyResponseDto { Key=200});
            }

            service.Status.CopyErrorsToModelState(ModelState, dto);

            SetupTraceInfo();       //Used to update the logs
            return BadRequest();
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        public JsonResult GetFilterSearchContent(DesciplineSortFilterPageOptions options)
        {
            var service = new DesciplineFilterDropdownService(_context);

            var traceIdent = HttpContext.TraceIdentifier; //This makes the logging display work

            return Json(
                new TraceIndentGeneric<IEnumerable<DropdownTuple>>(
                traceIdent,
                service.GetFilterDropDownValues(
                    options.FilterBy)));
        }

    }
}
