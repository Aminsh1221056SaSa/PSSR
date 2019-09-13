
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ContractorServices.Concrete;
using PSSR.Logic.Contractors;
using BskaGenericCoreLib;
using PSSR.UI.Helpers;
using PSSR.ServiceLayer.ContractorServices;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.CashHelper;
using PSSR.UI.Helpers.Http;
using PSSR.UI.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Web.Http;
using PSSR.UI.Helpers.Security;
using Microsoft.AspNetCore.Authorization;
using PSSR.DataLayer.EfClasses.Person;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiVersion("1.0")]
    public class ContractorController : BaseAdminController
    {
        private readonly EfCoreContext _context;
        private readonly IMasterDataCacheOperations _masterDataCache;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;

        public ContractorController(EfCoreContext context, IHttpClient clientService
            , IOptions<ApplicationSettings> settings, IMasterDataCacheOperations masterDataCache)
        {
            _context = context;
            _clientService = clientService;
            _masterDataCache = masterDataCache;
            _settings = settings;
        }

        [Authorize(Policy = "dataEventRecordsAdmin")]
        // GET: /<controller>/
        public async Task<IActionResult> Contractor(ContractorSortFilterPageOptions options)
        {
            var contractorService = new ListContractorService(_context);
            var contractors =await contractorService.SortFilterPage(options);
            SetupTraceInfo();           //Thsi makes the logging display work
            var viewModel = new ContractorListCombinedDto(options, contractors);
            await _masterDataCache.CreateMasterDataCacheAsync(User.GetCurrentUserDetails().Name, viewModel);
            return View(viewModel);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(ContractorListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetContractor([FromQuery] int contractorid)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Admin/GetContractor?contractorid={contractorid}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        //
        [Authorize(Policy = "dataEventRecordsAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContractor(ContractorDto model,
            [FromServices]IActionService<IPlaceContractorAction> service)
        {
            var contractor = service.RunBizAction<Contractor>(model);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return RedirectToAction("Contractor");
            }

            service.Status.CopyErrorsToModelState(ModelState, model);
            var viewModel = await _masterDataCache.GetMasterDataCacheAsync<ContractorSortFilterPageOptions>(User.GetCurrentUserDetails().Name);
            SetupTraceInfo();       //Used to update the logs
            return View("Contractor", viewModel);
        }

        [Authorize(Policy = "dataEventRecordsAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateContractor(ContractorDto model,
            [FromServices]IActionService<IUpdateContractorAction> service)
        {
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return RedirectToAction("Contractor");
            }

            service.Status.CopyErrorsToModelState(ModelState, model);
            var viewModel = await _masterDataCache.GetMasterDataCacheAsync<ContractorSortFilterPageOptions>(User.GetCurrentUserDetails().Name);
            SetupTraceInfo();       //Used to update the logs
            return View("Contractor",viewModel);
        }
    }
}
