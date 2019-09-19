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
using PSSR.DataLayer.EfClasses;
using PSSR.DataLayer.EfClasses.Person;
using PSSR.DataLayer.EfCode;
using PSSR.Logic.Persons;
using PSSR.ServiceLayer.Logger;
using PSSR.ServiceLayer.PersonService;
using PSSR.ServiceLayer.PersonService.Concrete;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using PSSR.ServiceLayer.Utils;
using PSSR.UI.Configuration;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers;
using PSSR.UI.Helpers.CashHelper;
using PSSR.UI.Helpers.Http;
using PSSR.UI.Helpers.Security;

namespace PSSR.UI.Areas.Admin.Controllers
{
  
    [Area("Admin")]
    [ApiVersion("1.0")]
    public class PersonController : BaseAdminController
    {
        private readonly EfCoreContext _context;
        private readonly IMasterDataCacheOperations _masterDataCache;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;

        public PersonController(EfCoreContext context, IMasterDataCacheOperations masterDataCache, IHttpClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _context = context;
            _masterDataCache = masterDataCache;
            _clientService = clientService;
            _settings = settings;
        }

        [Authorize(Policy = "dataEventRecordsAdmin")]
        public async Task<IActionResult> Persons(PersonSortFilterPageOptions options)
        {
            var listService = new ListPersonSystemService(_context);
            var projectService = new ListProjectService(_context);
            var projectSummary =await projectService.GetProjectSummary();
            var items= await Task.Run(() =>
            {
                return listService
                 .SortFilterPage(options).ToList();
            });

            SetupTraceInfo();           //Thsi makes the logging display work
            var viewModel = new PersonListCombinedDto(options, items,projectSummary);
            await _masterDataCache.CreateMasterDataCacheAsync(User.GetCurrentUserDetails().Name, viewModel);
            return View(viewModel);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(ServiceLayer.PersonService.PersonListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPerson(int personId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Admin/GetPerson?personId={personId}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        //
        [Authorize(Policy = "dataEventRecordsAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Persons(PersonDto model,
          [FromServices]IActionService<IPlacePersonAction> service)
        {
            var listService = new ListPersonSystemService(_context);

            if (listService.HasTakenNationalId(model.NationalId))
            {
                service.Status.AddError("The national id has already been used.");
            }

            if(!service.Status.HasErrors && ModelState.IsValid)
            {
                var person = service.RunBizAction<Person>(model);
                if (!service.Status.HasErrors)
                {
                    SetupTraceInfo();
                    return RedirectToAction("Persons");
                }
            }

            var viewModel = _masterDataCache.GetMasterDataCacheAsync<PersonListCombinedDto>(User.GetCurrentUserDetails().Name).Result;
            service.Status.CopyErrorsToModelState(ModelState, model);

            SetupTraceInfo();       //Used to update the logs

            return View(viewModel);
        }

        [Authorize(Policy = "dataEventRecordsAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePerson(PersonDto model,
          [FromServices]IActionService<IUpdatePersonAction> service)
        {
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return RedirectToAction("Persons");
            }

            service.Status.CopyErrorsToModelState(ModelState, model);

            SetupTraceInfo();       //Used to update the logs
            var viewModel = await _masterDataCache.GetMasterDataCacheAsync<PersonSortFilterPageOptions>(User.GetCurrentUserDetails().Name);
            return View("Persons", viewModel);
        }

        //
        [Authorize(Policy = "dataEventRecordsAdmin")]
        [HttpGet]
        public JsonResult GetFilterSearchContent(PersonSortFilterPageOptions options)
        {
            var service = new PersonFilterDropdownService(_context);

            var traceIdent = HttpContext.TraceIdentifier; //This makes the logging display work

            return Json(
                new TraceIndentGeneric<IEnumerable<DropdownTuple>>(
                traceIdent,
                service.GetFilterDropDownValues(
                    options.FilterBy)));
        }
    }
}