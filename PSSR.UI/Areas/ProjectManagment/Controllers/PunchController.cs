using BskaGenericCoreLib;
using Microsoft.AspNetCore.Mvc;
using PSSR.DataLayer.EfCode;
using PSSR.UI.Helpers;
using PSSR.UI.Models;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Web.Http;
using PSSR.Logic.Activityes;
using PSSR.ServiceLayer.PunchServices;
using PSSR.ServiceLayer.PunchServices.Concrete;
using System.Linq;
using PSSR.ServiceLayer.Logger;
using System.Collections.Generic;
using PSSR.ServiceLayer.Utils;
using PSSR.UI.Controllers;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using Microsoft.AspNetCore.Hosting;
using PSSR.ServiceLayer.ActivityServices;
using PSSR.UI.Helpers.Http;
using Microsoft.Extensions.Options;
using PSSR.UI.Configuration;
using Microsoft.AspNetCore.Authentication;
using PSSR.ServiceLayer.ActivityServices.Concrete;
using Microsoft.AspNetCore.Http;
using PSSR.Logic.Punches;
using PSSR.ServiceLayer.PunchTypeServices.Concrete;
using System;
using PSSR.UI.Helpers.Security;
using PSSR.UI.Helpers.CashHelper;
using PSSR.ServiceLayer.PunchCategoryServices.Concrete;
using Microsoft.AspNetCore.Authorization;
using PSSR.DataLayer.EfClasses.Projects.Activities;

namespace PSSR.UI.Areas.ProjectManagment.Controllers
{
    [Area("ProjectManagment")]
    [ApiVersion("1.0")]
    public class ActivityPunchController : BaseManagerController
    {
        private readonly EfCoreContext _context;
        private readonly IHostingEnvironment _enviroment;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        private readonly IMasterDataCacheOperations _masterDataCache;

        public ActivityPunchController(EfCoreContext context, IHostingEnvironment enviroment, IHttpClient clientService
            , IOptions<ApplicationSettings> settings, IMasterDataCacheOperations masterDataCache)
        {
            this._context = context;
            this._enviroment = enviroment;
            _clientService = clientService;
            _settings = settings;
            _masterDataCache = masterDataCache;
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        public async Task<IActionResult> PunchList(PunchSortFilterPageOptions options, Guid pid = default(Guid))
        {
            var user = User.GetCurrentUserDetails();
            if (pid != default(Guid))
            {
                _masterDataCache.SetUserCurrentProject(user.Name, pid);
            }
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);

            var punchService = new ListPunchService(_context);
            var activityList = (await punchService
               .SortFilterPage(options, cpid)).ToList();

            SetupTraceInfo();           //Thsi makes the logging display work
            return View(new PunchListCombinedDto(options, activityList));
        }

        [HttpGet]
        public IActionResult EditPunch(long id)
        {
            return View(id);
        }

        [HttpGet]
        public IActionResult CreatePunch()
        {
            return View();
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(CustomOptionModel<PunchSortFilterPageOptions, PunchListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PunchListSummary([FromQuery] string filterByOption, [FromQuery] string sortByOption
            , [FromQuery] string filterValue, [FromQuery]int pageNum, [FromQuery]int pageSize, [FromQuery] string query = "", [FromQuery]string prevCheckState = "")
        {
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerPunch/PunchListSummary?filterByOption=" + filterByOption + "&sortByOption=" + sortByOption + "&filterValue=" + filterValue
                + "&pageNum=" + pageNum + "&pageSize=" + pageSize + "&projectId="+cpid+"&query=" + query + "&prevCheckState=" + prevCheckState ,authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(PunchEditableListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActivityPunchs(long activityId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerPunch/GetActivityPunchs?activityId={activityId}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(PunchEditableListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPunchGoDetails(long punchId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerPunch/GetPunchGoDetails?punchId={punchId}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(PunchListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPunchDetails(long punchId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerPunch/GetPunchDetails?punchId={punchId}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(ActivityStatusHistoryListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPunchDocuments(long punchId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerPunch/GetPunchDocuments?punchId={punchId}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        //
        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        public IActionResult CreatePunch(Logic.Punches.PunchDto model,
        [FromServices]IActionService<Logic.Punches.IPlacePunchAction> service,
        [FromServices]IActionService<IUpdateActivityProgressAction> activityProgressService)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var Punch = service.RunBizAction<Punch>(model);
                if (!service.Status.HasErrors)
                {
                    activityProgressService.RunBizAction(Punch.Id);
                    if (!activityProgressService.Status.HasErrors)
                    {
                        transaction.Commit();
                        SetupTraceInfo();
                        return RedirectToAction("CreatePunch");
                    }
                    activityProgressService.Status.CopyErrorsToModelState(ModelState, model);
                }
            }

            service.Status.CombineErrors(activityProgressService.Status);
            service.Status.CopyErrorsToModelState(ModelState, model);
            SetupTraceInfo();       //Used to update the logs
            return View("CreatePunch");
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult AddPUnchToActivity([FromBody] Logic.Punches.PunchDto model,
        [FromServices]IActionService<Logic.Punches.IPlacePunchAction> service,
        [FromServices]IActionService<IUpdateActivityProgressAction> activityProgressService)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var Punch = service.RunBizAction<Punch>(model);

                if (!service.Status.HasErrors)
                {
                    activityProgressService.RunBizAction(Punch.Id);
                    if (!activityProgressService.Status.HasErrors)
                    {
                        transaction.Commit();
                        SetupTraceInfo();
                        return new ObjectResult(new SuccessfullyResponseDto { Key = 200, Value = "Punch success add to activity..." });
                    }
                }
            }
            service.Status.CombineErrors(activityProgressService.Status);
            service.Status.CopyErrorsToModelState(ModelState, model);
            SetupTraceInfo();       //Used to update the logs
            return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPut]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult UpdatePUnchToActivity([FromBody] Logic.Punches.PunchDto model,
         [FromServices]IActionService<Logic.Punches.IUpdatePunchDbAccess> service)
        {
            service.RunBizAction(model);
            
            service.Status.CopyErrorsToModelState(ModelState, model);
            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new SuccessfullyResponseDto { Key = 200, Value = "Punch success update..." });
            }
            SetupTraceInfo();       //Used to update the logs
            return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPut]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult UpdatePUnchGo([FromBody] Logic.Punches.PunchGoDto model,
         [FromServices]IActionService<Logic.Punches.IUpdatePunchGoAction> service,
         [FromServices]IActionService<IUpdateActivityProgressPunchModifyAction> activityProgressService)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                service.RunBizAction(model);
                activityProgressService.RunBizAction(model.Id);

                if (!service.Status.HasErrors)
                {
                    if (!activityProgressService.Status.HasErrors)
                    {
                        transaction.Commit();
                        SetupTraceInfo();
                        return new ObjectResult(new SuccessfullyResponseDto { Key = 200, Value = "punch successfully update!!!" });
                    }
                }
            }

            activityProgressService.Status.CopyErrorsToModelState(ModelState, model);
            service.Status.CopyErrorsToModelState(ModelState, model);

            SetupTraceInfo();       //Used to update the logs
            return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpDelete]
        [Route("poec/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult DeletePunch(long id,[FromServices]IActionService<Logic.Punches.IDeletePunchAction> service
            , [FromServices]IActionService<IUpdateActivityProgressDeleteAction> activityProgressService)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                activityProgressService.RunBizAction(id);
                service.RunBizAction(id);

                if (!activityProgressService.Status.HasErrors)
                {
                    if (!service.Status.HasErrors)
                    {
                        transaction.Commit();
                        SetupTraceInfo();
                        return new ObjectResult(new SuccessfullyResponseDto { Key = 200, Value = "punch successfully deleted!!!" });
                    }
                }
            }

            service.Status.CombineErrors(activityProgressService.Status);

            service.Status.CopyErrorsToModelState(ModelState, id);

            SetupTraceInfo();       //Used to update the logs
            return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value =service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        public async Task<IActionResult> CreateMainDocumentFile(ActivityDocumentDto model,
           [FromServices]IActionService<IPlaceActivityDocumentAction> service)
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            var punchService = new ListPunchService(_context);
            var result = await punchService.GetPunchDetails(model.PunchId.Value);

            FormDocumentFileHelper docHelper = new FormDocumentFileHelper();

            if (model.File != null && model.File.Length > 0)
            {
                string fileName1 = await docHelper.SaveActivityDocument(result.ActivityId, project.Id.ToString(), model.File, _enviroment);
                model.FilePath = fileName1;
                model.ActivityId = result.ActivityId;
                service.RunBizAction(model);
            }

            SetupTraceInfo();
            return RedirectToAction("EditPunch", new { id = model.PunchId.Value });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UploadExcel(IFormFile file,
          [FromServices]IActionService<IPlcaePunchBulkAction> service,
          [FromServices]IActionService<IUpdateActivityBulkAction> activityProgressService)
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            var activityService = new ListActivityService(_context);
            var punchTypeService = new ListPunchTypeService(_context);
            var pCategoryService = new ListPunchCategoryService(_context);

            var allActivityes = await activityService.GetProjectActivitiesIncludePunches(project.Id);

            var acDic = allActivityes.DistinctBy(s => s.ActivityCode)
                .ToDictionary(s => s.ActivityCode, s => s.Id);

            var allpunchesType = await punchTypeService.GetProjectPunches(project.Id);
            var typeDic= allpunchesType.ToDictionary(x => x.Id, x => x.Name);
            var categoryDic= (await  pCategoryService.GetProjectPuncheCategories(project.Id)).ToDictionary(x => x.Id, x => x.Name);
            if (file != null)
            {
                ExcelFileConverterHelper converter = new ExcelFileConverterHelper();

                var model = await converter.ParsePunchExcel(file, acDic, typeDic,categoryDic);

                if (string.IsNullOrWhiteSpace(model.Item1))
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            var pIds = model.Item2.Where(s => !s.ClearDate.HasValue)
                                .OrderBy(s => s.PunchTypeId).ToLookup(x=>x.ActivityId,x=>x.PunchTypeId);

                            var updateActivity = new List<Activity>();

                            Parallel.ForEach(pIds, ac =>
                             {
                                 var activity = allActivityes.First(s => s.Id == ac.Key);
                                 float precentage = 0;
                                 var ptypeDistinict = ac.DistinctBy(s => s).ToList();
                                 int counter = ptypeDistinict.Count();
                                 for (int i = 0; i < counter; i++)
                                 {
                                     var pType = allpunchesType.First(s => s.Id == ptypeDistinict[i]);
                                     var workPackage = pType.WorkPackages.FirstOrDefault(s => s.WorkPackageId == activity.WorkPackageId);
                                     if (workPackage != null)
                                     {
                                         if (!activity.Punchs.Any(s => s.PunchTypeId == ptypeDistinict[i]))
                                         {
                                             precentage = activity.Progress - workPackage.Precentage;
                                             activity.UpdateActivityProgress(precentage);
                                         }
                                         else
                                         {
                                             if (activity.Progress >= 100)
                                             {
                                                 precentage = activity.Progress - workPackage.Precentage;
                                                 activity.UpdateActivityProgress(precentage);
                                             }
                                         }
                                     }
                                 }
                                 updateActivity.Add(activity);
                             });

                            activityProgressService.RunBizAction(updateActivity);
                            if (model.Item2.Any())
                                service.RunBizAction(model.Item2);
                            if (!service.Status.HasErrors)
                            {
                                if(!activityProgressService.Status.HasErrors)
                                {
                                    transaction.Commit();
                                }
                            }

                            if (!service.Status.HasErrors)
                            {
                                SetupTraceInfo();
                                return new ObjectResult(new SuccessfullyResponseDto { Key = 200 });
                            }
                        }
                        catch (Exception ex)
                        {
                            service.Status.AddError(ex.Message);
                            service.Status.CopyErrorsToModelState(ModelState, "punch");
                            activityProgressService.Status.CopyErrorsToModelState(ModelState, "punch");
                        }
                    }
                }
                else
                {
                    service.Status.AddError(model.Item1);
                }
            }
            else
            {
                service.Status.AddError("please select a file", "Form Dictionary");
            }

            SetupTraceInfo();       //Used to update the logs
            return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        public JsonResult GetFilterSearchContent(PunchSortFilterPageOptions options)
        {
            var service = new PunchFilterDropdownService(_context);

            var traceIdent = HttpContext.TraceIdentifier; //This makes the logging display work

            return Json(
                new TraceIndentGeneric<IEnumerable<DropdownTuple>>(
                traceIdent,
                service.GetFilterDropDownValues(
                    options.FilterBy)));
        }
    }
}
