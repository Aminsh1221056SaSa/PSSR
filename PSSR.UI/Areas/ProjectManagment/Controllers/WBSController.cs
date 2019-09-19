
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ProjectServices;
using System.Net;
using AutoMapper;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using PSSR.UI.Models;
using PSSR.Logic.Projects;
using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.UI.Helpers;
using System.Collections.Generic;
using System;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.Http;
using Microsoft.Extensions.Options;
using PSSR.UI.Configuration;
using Microsoft.AspNetCore.Authentication;
using PSSR.Common;
using PSSR.ServiceLayer.SubSystemServices.Concrete;
using System.Linq;
using PSSR.UI.Helpers.Security;
using PSSR.UI.Helpers.CashHelper;
using System.Data;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.ProjectManagment.Controllers
{
    [Area("ProjectManagment")]
    [ApiVersion("1.0")]
    public class WBSController : BaseManagerController
    {
        private readonly EfCoreContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        private IOptions<SqlConnectionHelper> _sqlsettings;
        private readonly IMasterDataCacheOperations _masterDataCache;
        private readonly IDatabaseService _databaseService;

        //private readonly IHubContext<WBSRoadMapHub> _wbsRoadMapHub;
        public WBSController(EfCoreContext context, IMapper mapper
            , IHttpClient clientService
            , IOptions<ApplicationSettings> settings, IMasterDataCacheOperations masterDataCache, IDatabaseService databaseService
            , IOptions<SqlConnectionHelper> sqlsettings)
        {
            _context = context;
            _mapper = mapper;
            _clientService = clientService;
            _settings = settings;
            _masterDataCache = masterDataCache;
            this._databaseService = databaseService;
            this._sqlsettings = sqlsettings;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(ProjectWBSListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectWBSTree()
        {
            var projectservice = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectservice.GetProject(cpid);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerWbs/GetProjectWBSTree?projectId={project.Id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(ProjectWBSListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectWBSActivityTree([FromQuery]long parentId)
        {
            var projectservice = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectservice.GetProject(cpid);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerWbs/GetProjectWBSActivityTree?parentId={parentId}&projectId={project.Id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(ProjectWBSListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectWBS([FromQuery] long wbsId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerWbs/GetProjectWBS?wbsId={wbsId}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        //
        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateProjectWBS([FromBody] ProjectWBSDto model,
         [FromServices]IActionService<IPlaceProjectWBSAction> service)
        {
            var projectService = new ListProjectService(_context);
            var wbsService = new ListWBSService(_context, _mapper);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            bool haveError = false;
            if (project == null)
            {
                service.Status.AddError("Project not valid!!!", "Project");
                haveError = true;
            }

            var parent = await wbsService.GetProjectWBS(model.ParentId.Value);

            if (parent==null)
            {
                service.Status.AddError("WBS Parent Not valid!!!", "Project");
                haveError = true;
            }

            if (wbsService.hasWbSChildDublicate(parent.Id, model.Type, model.Name))
            {
                service.Status.AddError("Duplicated WBS Type for Selected Parent!!!", "Project");
                haveError = true;
            }

            if (haveError)
            {
                return new ObjectResult(new SuccessfullyResponseModelDto<ProjectWBSListDto> { Key = -1, Value = service.Status.GetAllErrors() });
            }

            //if (wbsService.wbsHasAnyActivity(model.TargetId, model.Type, project.Id))
            //{
            //    model.CalculationType = Common.WfCalculationType.Automatic;
            //}
            //else
            //{
            //    model.CalculationType = Common.WfCalculationType.Manual;
            //}
            model.CalculationType = Common.WfCalculationType.Automatic;
            model.WBSCode = wbsService.GetProjectWBSNextChildCode(parent.Id, parent.WBSCode);

            model.ProjectId = project.Id;
            var newCreated = service.RunBizAction<ProjectWBS>(model);

            if (!service.Status.HasErrors)
            {
                var items = new List<ProjectWBSListDto>();

                items.Add(new ProjectWBSListDto
                {
                    Id = newCreated.Id,
                    ParentId = newCreated.ParentId,
                    TargetId = newCreated.TargetId,
                    Type = newCreated.Type,
                    WBSCode = newCreated.WBSCode,
                    WF = newCreated.WF,
                    Name = newCreated.Name,
                    CalculationType=newCreated.CalculationType
                });

                if (model.Type == WBSType.System)
                {
                    var subsystemService = new ListProjectSubSystemService(_context);
                    var subSystems = await subsystemService.GetSubSystemBySystem(model.TargetId);

                    if (subSystems.Any())
                    {
                        subSystems.ForEach(sb =>
                        {
                            var addModel = new ProjectWBSDto
                            {
                                Name = $"{sb.Code}({sb.Description})",
                                ParentId = newCreated.Id,
                                ProjectId = project.Id,
                                TargetId = sb.Id,
                                Type = WBSType.SubSystem,
                                WF = 0,
                                CalculationType = WfCalculationType.Automatic
                            };

                            addModel.WBSCode = wbsService.GetProjectWBSNextChildCode(newCreated.Id, newCreated.WBSCode);
                            var newSubCreated = service.RunBizAction<ProjectWBS>(addModel);
                            if (!service.Status.HasErrors)
                            {
                                items.Add(new ProjectWBSListDto
                                {
                                    Id = newSubCreated.Id,
                                    ParentId = newSubCreated.ParentId,
                                    TargetId = newSubCreated.TargetId,
                                    Type = newSubCreated.Type,
                                    WBSCode = newSubCreated.WBSCode,
                                    WF = newSubCreated.WF,
                                    Name = newSubCreated.Name
                                });
                            }
                        });
                    }
                }

                SetupTraceInfo();
                return new ObjectResult(new SuccessfullyResponseModelDto<List<ProjectWBSListDto>>
                {
                    Model = items,
                    Key = 200,
                    Value = "Create Project WBS success!!"
                });
            }

            service.Status.CopyErrorsToModelState(ModelState, model);
            return new ObjectResult(new SuccessfullyResponseModelDto<ProjectWBSListDto> { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPut]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult EditProjectWBS([FromBody] ProjectWBSDto model,
        [FromServices]IActionService<IUpdateProjectWBSAction> service)
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            bool haveError = false;
            if (project == null)
            {
                service.Status.AddError("Project not valid!!!", "Project");
                haveError = true;
            }

            if (haveError)
            {
                return new ObjectResult(new SuccessfullyResponseModelDto<ProjectWBSListDto> { Key = -1, Value = service.Status.GetAllErrors() });
            }

            model.ProjectId = project.Id;
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return new ObjectResult(new SuccessfullyResponseModelDto<ProjectWBSListDto>
                {
                    Key = 200,
                    Value = "Update Project WBS success!!"
                });
            }

            service.Status.CopyErrorsToModelState(ModelState, model);
            return new ObjectResult(new SuccessfullyResponseModelDto<ProjectWBSListDto> { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPut]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EditProjectWBSWF([FromBody] CustomUpdateModel<float> model,
        [FromServices]IActionService<IUpdateProjectWBSWFAction> service)
        {
            var projectService = new ListProjectService(_context);
            var wbsService = new ListWBSService(_context, _mapper);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            bool haveError = false;
            if (project == null)
            {
                service.Status.AddError("Project not found!!!", "Project");
                haveError = true;
            }

            var projectWbs =await wbsService.GetProjectWBS(model.Key);
            if (projectWbs == null)
            {
                service.Status.AddError("Project wbs not found!!!", "Project wbs");
                haveError = true;
            }

            if (projectWbs.Type == Common.WBSType.Project)
            {
                service.Status.AddError("Project wbs wf not allowed for change!!!", "Project wbs");
                haveError = true;
            }

            if (!wbsService.IsValidWBSWf(model.Value,projectWbs.ParentId??0,projectWbs.Id))
            {
                service.Status.AddError("Weight factor not valid!!!", "Project wbs");
                haveError = true;
            }

            if (haveError)
            {
                return new ObjectResult(new SuccessfullyResponseModelDto<ProjectWBSListDto> { Key = -1, Value = service.Status.GetAllErrors() });
            }

            var updateModel = new ProjectWBSDto
            {
                Id=projectWbs.Id,
                WF=model.Value
            };

            updateModel.ProjectId = project.Id;
            service.RunBizAction(updateModel);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return new ObjectResult(new SuccessfullyResponseModelDto<ProjectWBSListDto>
                {
                    Key = 200,
                    Value = "Update Project WBS wf success!!"
                });
            }

            service.Status.CopyErrorsToModelState(ModelState, model);
            return new ObjectResult(new SuccessfullyResponseModelDto<ProjectWBSListDto> { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpDelete("APSE/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult DeleteProjectWBS(long id,
        [FromServices]IActionService<IDeleteProjectWBSAction> service)
        {
            var wBSService = new ListWBSService(_context, _mapper);
            bool haveError = false;
            if (wBSService.WBSNodeHasAnyChild(id))
            {
                service.Status.AddError("WBS node has any child!!!", "Project");
                haveError = true;
            }
            //if (projectService.WbsNodeHasAnyActivity(id))
            //{
            //    service.Status.AddError("WBS node has any Activity!!!", "Project");
            //    haveError = true;
            //}
            
            if (haveError)
            {
                return new ObjectResult(new SuccessfullyResponseModelDto<ProjectWBSListDto> { Key = -1, Value = service.Status.GetAllErrors() });
            }

            service.RunBizAction(id);

            if (!service.Status.HasErrors)
            {
                SetupTraceInfo();
                return new ObjectResult(new SuccessfullyResponseModelDto<ProjectWBSListDto>
                {
                    Key = 200,
                    Value = "Delete Project WBS success!!"
                });
            }

            service.Status.CopyErrorsToModelState(ModelState, "projectWBS");
            return new ObjectResult(new SuccessfullyResponseModelDto<ProjectWBSListDto> { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPut]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAllActivityWf([FromBody] WFCalTypeModel model)
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);
            var listService = new ListWBSService(_context,_mapper);
            var calDic = await listService.calculateWFForAllActivity(project.Id);
            if (calDic == null)
            {
                return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value = "Wbs tree not Defined!!!" });
            }
            DataTable calwbs = null;
            if (model.CalType == 1002)
            {
                calwbs = await listService.CalculateWFForAllWBS(project.Id,false);
            }
            else if(model.CalType==1003)
            {
                calwbs = await listService.CalculateWFForAllWBS(project.Id, true);
            }

            _databaseService.ConnectionString = _sqlsettings.Value.DefaultConnection;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _databaseService.ExecuteNonQuery("dbo.UpdateTaskWF", CommandType.StoredProcedure, calDic);
                    if(calwbs!=null)
                    {
                        _databaseService.ExecuteNonQuery("dbo.UpdateWBSWF", CommandType.StoredProcedure, calwbs);
                    }
                    
                    return new ObjectResult(new SuccessfullyResponseDto { Key = 200, Value = "wf has been updated..." });
                }
                catch (Exception ex)
                {
                    return new ObjectResult(new SuccessfullyResponseDto { Key = -1, Value = ex.Message});
                }
            }
        }
    }
}
