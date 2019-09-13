using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Net;
using System.IO;
using BskaGenericCoreLib;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ActivityServices.Concrete;
using PSSR.ServiceLayer.ActivityServices;
using PSSR.ServiceLayer.Logger;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using PSSR.ServiceLayer.Utils;
using PSSR.ServiceLayer.FormDictionaryServices.Concrete;
using PSSR.ServiceLayer.ValueUnits.Concrete;
using PSSR.Logic.Activityes;
using PSSR.UI.Helpers;
using PSSR.ServiceLayer.ProjectServices;
using PSSR.Common;
using PSSR.UI.Models;
using PSSR.UI.Hubs;
using PSSR.ServiceLayer.RoadMapServices.Concrete;
using PSSR.ServiceLayer.ProjectSystemServices.Concrete;
using PSSR.ServiceLayer.DesciplineServices.Concrete;
using PSSR.ServiceLayer.SubSystemServices.Concrete;
using PSSR.UI.Configuration;
using PSSR.UI.Controllers;
using PSSR.ServiceLayer.WorkPackageSteps.Concrete;
using PSSR.UI.Helpers.Http;
using Microsoft.AspNetCore.Authentication;
using PSSR.UI.Helpers.Security;
using PSSR.UI.Helpers.CashHelper;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using PSSR.DataLayer.EfClasses.Projects.Activities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.ProjectManagment.Controllers
{
    [Area("ProjectManagment")]
    [ApiVersion("1.0")]
    public class ActivityController : BaseManagerController
    {
        private readonly EfCoreContext _context;
        private readonly IHostingEnvironment _enviroment;
        private readonly IHubContext<WBSRoadMapHub> _wbsHubContext;
        private readonly IDatabaseService _databaseService;
        private IOptions<SqlConnectionHelper> _sqlsettings;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        private readonly IMasterDataCacheOperations _masterDataCache;

        public ActivityController(EfCoreContext context, 
            IHostingEnvironment enviroment, IHubContext<WBSRoadMapHub> wbsHubcontext,
            IDatabaseService databaseService, IOptions<SqlConnectionHelper> sqlsettings, IHttpClient clientService
            , IOptions<ApplicationSettings> settings, IMasterDataCacheOperations masterDataCache)
        {
            _context = context;
            this._enviroment = enviroment;
            this._wbsHubContext = wbsHubcontext;
            this._databaseService = databaseService;
            _sqlsettings = sqlsettings;
            _clientService = clientService;
            _settings = settings;
            _masterDataCache = masterDataCache;
        }

        [HttpGet]
        public IActionResult ActivityList(ActivitySortFilterPageOptions model,Guid pid = default(Guid))
        {
            var user = User.GetCurrentUserDetails();
            if(pid!=default(Guid))
            {
                _masterDataCache.SetUserCurrentProject(user.Name, pid);
            }
            return View(model);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        public IActionResult EditActivity(long id)
        {
            return View(id);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        public async Task<IActionResult> CreateActivity()
        {
            var projectservice = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectservice.GetProject(cpid);

            var activityService =new ListActivityService(_context);
            var listDesciplineService = new ListDesciplineService(_context);
            var valueUnitList = new ListValueUnitService(_context);
            var roadMapService = new ListWorkPackageService(_context);
            var listSubSystemService = new ListProjectSystemService(_context);

            long nextCode = await activityService.GetActivityNextCode();
            ViewBag.Desciplines = await listDesciplineService.GetAllDesciplines();
            ViewBag.valueUnits = await valueUnitList.GetValueUnitDtos();
            ViewBag.WorkPackages = await roadMapService.GetRoadMapsAsync();
            ViewBag.Locations = await roadMapService.GetLocationsAsync();
            ViewBag.Systems = await listSubSystemService.GetProjectSystems(project.Id);

            ViewBag.MethodName = "CreateActivity";

            return View(new ActivityListDetailsDto {ActivityCode=nextCode.ToString(),
                Condition =ActivityCondition.Normal,ValueUnitNum=1,Status=ActivityStatus.NotStarted});
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        public async Task<IActionResult> EditActivityDetails(long id)
        {
            var projectservice = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectservice.GetProject(cpid);

            var listService =
                    new ListActivityService(_context);
            var model = await listService.GetActivity(id);
            var listDesciplineService = new ListDesciplineService(_context);

            ViewBag.Desciplines = await listDesciplineService.GetAllDesciplines();

            var valueUnitList =
                  new ListValueUnitService(_context);
            ViewBag.valueUnits = await valueUnitList.GetValueUnitDtos();

            var roadMapService = new ListWorkPackageService(_context);
            var listSubSystemService =
                new ListProjectSystemService(_context);

            ViewBag.WorkPackages = await roadMapService.GetRoadMapsAsync();
            ViewBag.Locations = await roadMapService.GetLocationsAsync();
            ViewBag.Systems = await listSubSystemService.GetProjectSystems(project.Id);

            ViewBag.MethodName = "EditActivity";

            return View(model);
        }

        //api getDatatable format
        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(List<ActivityListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActivityDataTable([FromBody] JqueryDataTablesParameters param)
        {
            var user = User.GetCurrentUserDetails();
            await _masterDataCache.CreateMasterDataCacheAsync(user.Name + "GetActivityDataTable", param);
            var activityService = new ListActivityService(_context);
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var results = await activityService.SortFilterPageDataTable(param, cpid);
            return new JsonResult(new JqueryDataTablesResult<ActivityListDataTableDto>
            {
                Draw = param.Draw,
                Data = results.Items,
                RecordsFiltered = results.TotalSize,
                RecordsTotal = results.TotalSize
            });
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(CustomOptionModel<ActivitySortFilterPageOptions, ActivityListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ActivityListSummary([FromQuery] string filterValue, [FromQuery]int page, 
            [FromQuery]int pageSize, [FromQuery] string search = "", [FromQuery]string prevCheckState = "", 
            [FromQuery] string filterByOption="0", [FromQuery] string sortByOption="0")
        {
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerActivity/ActivityListSummary?filterByOption=" + filterByOption + "&sortByOption=" + sortByOption + "&filterValue=" + filterValue
                + "&pageNum=" + page + "&pageSize=" + pageSize + "&projectId="+cpid+"&query=" + search + "&prevCheckState=" + prevCheckState,authorizationToken: accessToken);

            return new ObjectResult(content);
        }


        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(CustomOptionModel<ActivitySortFilterPageOptions, ActivityListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ActivityListSummaryByWorkDescipline([FromQuery] int workId, [FromQuery] int desId
           , [FromQuery] string filterValue, [FromQuery]int pageNum, [FromQuery]int pageSize, [FromQuery] string query = "", [FromQuery]string prevCheckState = ""
            , [FromQuery] string filterByOption = "0", [FromQuery] string sortByOption = "0")
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerActivity/ActivityListSummaryByWorkDescipline?workId=" + workId + "&desId=" + desId +"&filterByOption =" + filterByOption + "&sortByOption=" + sortByOption + "&filterValue=" + filterValue
                + "&pageNum=" + pageNum + "&pageSize=" + pageSize + "&query=" + query + "&prevCheckState=" + prevCheckState, authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(ActivityListDetailsDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActivityDetails(long id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerActivity/GetActivityDetails?id={id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(ProjectWBSListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActivityWBSTree(long activityWBsId)
        {
            var projectservice = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectservice.GetProject(cpid);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerActivity/GetActivityWBSTree?activityWBsId={activityWBsId}&projectId={project.Id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(ActivityStatusHistoryListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActivityStatusHistory(long activityId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerActivity/GetActivityStatusHistory?activityId={activityId}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        //
        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(ActivityStatusHistoryListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActivityDocuments(long activityId, [FromServices]IActionService<IPlaceActivityDocumentAction> service)
        {
            var activityService = new ListActivityService(_context);
            var result = await activityService.GetActivity(activityId);

            if (result.FormDictionaryId.HasValue)
            {
                FormDocumentFileHelper docHelper = new FormDocumentFileHelper();
                if (!await activityService.HasMainDocument(result.Id))
                {
                    var projectService = new ListProjectService(_context);
                    var user = User.GetCurrentUserDetails();
                    var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
                    var project = projectService.GetProject(cpid);

                    var formService = new ListFormDictionaryService(_context);
                    var form = await formService.GetformDictionary(result.FormDictionaryId.Value);

                    string fileName = docHelper.MoveFormDocToActivityFolder(form.Code, result.Id, project.Id.ToString(), _enviroment);
                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        var newdocModel = new ActivityDocumentDto
                        {
                            Description = "Main Document",
                            FilePath = fileName,
                            PunchId = null,
                            ActivityId = result.Id
                        };

                        service.RunBizAction(newdocModel);
                    }
                }
            }
            var lstHistory = await activityService.GetDocuments(activityId);
            return new ObjectResult(lstHistory);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        public async Task<IActionResult> DownloadDocumentFile(long documentId)
        {
            var activityService = new ListActivityService(_context);
            var filePath = await activityService.GetActivityDocumentFilePath(documentId);
            var contentType = "APPLICATION/octet-stream";
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("Can't find specific file.");
            }

            var fileName = Path.GetFileName(filePath);
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType, fileName);
        }

        //
        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateActivity(ActivityDto model,
        [FromServices]IActionService<IPlaceActivityAction> service)
        {
            var projectService = new ListProjectService(_context);
            var wbsService = new ListWBSService(_context, null);

            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            if (project == null)
            {
                service.Status.AddError("project not valid!!!", "Project");
            }

            if (!service.Status.HasErrors)
            {
                model.Condition = ActivityCondition.Normal;
                model.Status = ActivityStatus.NotStarted;

                var activity = service.RunBizAction<Activity>(model);

                if(!service.Status.HasErrors)
                {
                    var activityService = new ListActivityService(_context);
                    var mActivity = await activityService.GetActivity(activity.Id);
                    //calculate wf
                    var calculates = await wbsService.CalculateActivityWBSOnTableAction(mActivity, project.Id);
                    if(calculates!=null && calculates.Rows.Count>0)
                    {
                        _databaseService.ConnectionString = _sqlsettings.Value.DefaultConnection;
                        _databaseService.ExecuteNonQuery("dbo.UpdateTaskWF", CommandType.StoredProcedure, calculates);
                    }
                    SetupTraceInfo();
                    return RedirectToAction("CreateActivity");
                }
            }

            service.Status.CopyErrorsToModelState(ModelState, model);

            SetupTraceInfo();       //Used to update the logs
            return View("CreateActivity");
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditActivity(ActivityDto model,
        [FromServices]IActionService<IUpdateActivityAction> service)
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);
            if (project == null)
            {
                service.Status.AddError("project not valid!!!", "Project");
            }

            if (!service.Status.HasErrors)
            {
                service.RunBizAction(model);
                SetupTraceInfo();
                return RedirectToAction("EditActivityDetails", new { id = model.Id });
            }

            service.Status.CopyErrorsToModelState(ModelState, model);

            SetupTraceInfo();       //Used to update the logs
            return View("EditActivityDetails");
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteActivity(long id, 
            [FromServices]IActionService<IUpdateActivityStatusAction> service)
        {
            var projectService = new ListProjectService(_context);
            var wbsService = new ListWBSService(_context,null);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            var activityService = new ListActivityService(_context);
            var activity = await activityService.GetActivity(id);

            if (activity != null)
            {
                var model = new ActivityStatusUpdateDto
                {
                    Condition = ActivityCondition.Normal,
                    CreateDate = DateTime.Now,
                    HoldBy = ActivityHolBy.NoHold,
                    Id = activity.Id,
                    Status=ActivityStatus.Delete,
                };

                service.RunBizAction(model);

                if (!service.Status.HasErrors)
                {
                    //calculate wf
                    var calculates = await wbsService.CalculateActivityWBSOnTableAction(activity,project.Id);
                    if (calculates != null && calculates.Rows.Count > 0)
                    {
                        _databaseService.ConnectionString = _sqlsettings.Value.DefaultConnection;
                        _databaseService.ExecuteNonQuery("dbo.UpdateTaskWF", CommandType.StoredProcedure, calculates);
                    }
                    return RedirectToAction("ActivityList");
                }

                service.Status.CopyErrorsToModelState(ModelState, "Activity");
            }

            return View("EditActivityDetails");
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

            var listService = new ListActivityService(_context);
            var result = await listService.GetActivity(model.ActivityId);

            if (result.FormDictionaryId.HasValue)
            {
                FormDocumentFileHelper docHelper = new FormDocumentFileHelper();
               
                if (model.File != null && model.File.Length > 0)
                {
                    string fileName1 = await docHelper.SaveActivityDocument(result.Id, project.Id.ToString(), model.File, _enviroment);
                    model.FilePath = fileName1;

                    service.RunBizAction(model);
                }
            }

            SetupTraceInfo();
            return RedirectToAction("EditActivity", new { id = model.ActivityId });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPut]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult UpdateActivityStatus([FromBody] ActivityStatusUpdateDto model,
         [FromServices]IActionService<IUpdateActivityStatusAction> service)
        {
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                //await SendMessageToSignalR(activity.ProjectWbsId,project.Id);
                SetupTraceInfo();
                return new ObjectResult(new SuccessfullyResponseDto { Key=200,Value="Activity Status Success Update..."});
            }

            service.Status.CopyErrorsToModelState(ModelState, model);

            SetupTraceInfo();       //Used to update the logs
            return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPut]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult UpdateActivityPlane([FromBody]ActivityPlaneDto model,
            [FromServices]IActionService<IUpdateActivityPlaneAction> service)
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);
            model.ProjectId = project.Id;

            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                //await SendMessageToSignalR(activity.ProjectWbsId,project.Id);
                SetupTraceInfo();
                return new ObjectResult(new SuccessfullyResponseDto { Key = 200, Value = "Activity Plane Success Update..." });
            }

            service.Status.CopyErrorsToModelState(ModelState, model);

            SetupTraceInfo();       //Used to update the logs
            return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UploadExcel(IFormFile file,
          [FromServices]IActionService<IPlcaeActivityBulkAction> insertservice, 
          [FromServices]IActionService<IUpdateActivityBulkAction> updateservice)
        {
            if (file != null)
            {
                ExcelFileConverterHelper converter = new ExcelFileConverterHelper();

                var roadMapService = new ListWorkPackageService(_context);
                var desciplineList = new ListDesciplineService(_context);
                var listformService = new ListFormDictionaryService(_context);
                var listsubsystemservice = new ListProjectSubSystemService(_context);
                var listSystemService = new ListProjectSystemService(_context);
                var projectService = new ListProjectService(_context);
                var activityService = new ListActivityService(_context);
                var valueUnitList = new ListValueUnitService(_context);
                var workPackageStepList = new WorkPackageStepService(_context);

                var user = User.GetCurrentUserDetails();
                var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
                var project = projectService.GetProject(cpid);
                if (project == null)
                {
                    insertservice.Status.AddError("Project Not Found!!!", "activity");
                }

                var workPackages = await roadMapService.GetRoadMapsAsync();
                if (!workPackages.Any())
                {
                    insertservice.Status.AddError("Not available WorkPackage!!!", "activity");
                }
                var locations = await roadMapService.GetLocationsAsync();
                if (!locations.Any())
                {
                    insertservice.Status.AddError("Not available location!!!", "activity");
                }

                var desciplines = await desciplineList.GetAllDesciplinesIdToFormCode();
                if (!desciplines.Any())
                {
                    insertservice.Status.AddError("Not available desciplines!!!", "activity");
                }

                var systems = await listSystemService.GetProjectSystems(project.Id);
                if(!systems.Any())
                {
                    insertservice.Status.AddError("Not available systems!!!", "activity");
                }

                var subsystems = await listsubsystemservice.GetProjectSubSystems(project.Id);
                if (!subsystems.Any())
                {
                    insertservice.Status.AddError("Not available subsystems!!!", "activity");
                }

                var valueUnits =  valueUnitList.GetAllValueUnits().ToList();
                if (!valueUnits.Any())
                {
                    insertservice.Status.AddError("Not available valueUnits!!!", "activity");
                }

                var wSteps = await workPackageStepList.GetWorkPackageSteps();
                if(!wSteps.Any())
                {
                    insertservice.Status.AddError("Not available workpackage step!!!", "activity");
                }

                if (!insertservice.Status.HasErrors)
                {
                    var workDic = workPackages.DistinctBy(x => x.Title).ToDictionary(x => x.Title.ToUpper(), x => x.Id);
                    var locaDic = locations.DistinctBy(x => x.Title).ToDictionary(x => x.Title.ToUpper(), x => x.Id);
                    var valUnitDic = valueUnits.DistinctBy(x => x.Name).ToDictionary(x => x.Name.ToUpper(), x => x.Id);
                    var systemDic = systems.DistinctBy(x => x.Title).ToDictionary(x => x.Title.ToUpper(), x => x.Id);
                    var subsytemDic = subsystems.DistinctBy(x => x.Title).ToDictionary(x => x.Title.ToUpper(), x => new Tuple<long, int>(x.Id, x.HId));
                    var workStepDic = wSteps.DistinctBy(x => x.Title).ToDictionary(x => x.Title.ToUpper(), x => x.Id);

                    var model = converter.ParseActivityExcel(file, valUnitDic, workDic, locaDic,
                        desciplines, systemDic, subsytemDic,workStepDic);

                    if (string.IsNullOrWhiteSpace(model.Item1))
                    {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                if(model.Item2.Any())
                                insertservice.RunBizAction(model.Item2);
                                if (!insertservice.Status.HasErrors)
                                {
                                    transaction.Commit();
                                    SetupTraceInfo();
                                    return new ObjectResult(new SuccessfullyResponseDto { Key = 200 });
                                }
                            }
                            catch(Exception ex)
                            {
                                insertservice.Status.AddError(ex.Message);
                                updateservice.Status.CopyErrorsToModelState(ModelState, "punch");
                                insertservice.Status.CopyErrorsToModelState(ModelState, "punch");
                            }
                        }
                            
                    }
                    else
                    {
                        insertservice.Status.AddError(model.Item1);
                    }
                }
            }
            else
            {
                insertservice.Status.AddError("please select a file", "Form Dictionary");
            }

            SetupTraceInfo();       //Used to update the logs
            return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value = insertservice.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        public JsonResult GetFilterSearchContent(ActivitySortFilterPageOptions options)
        {
            var service = new ActivityFilterDropdownService(_context);

            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            var traceIdent = HttpContext.TraceIdentifier; //This makes the logging display work

            return Json(
                new TraceIndentGeneric<IEnumerable<DropdownTuple>>(
                traceIdent,
                service.GetFilterDropDownValues(
                    options.FilterBy,project.Id)));
        }
        
        private async Task SendMessageToSignalR(long activitywbsId,Guid projectId)
        {
            //var listService =
            //      new ListActivityService(_context);

            //string rItems = JsonConvert.SerializeObject(items, Formatting.Indented,
            //new JsonSerializerSettings
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //});
            //await _wbsHubContext.Clients.All.SendAsync("activityProgressChanged", rItems);
        }

    }
}
