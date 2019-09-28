
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;

using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ActivityServices;
using PSSR.ServiceLayer.ActivityServices.Concrete;
using PSSR.ServiceLayer.DesciplineServices;
using PSSR.ServiceLayer.DesciplineServices.Concrete;
using PSSR.ServiceLayer.FormDictionaryServices;
using PSSR.ServiceLayer.FormDictionaryServices.Concrete;
using PSSR.ServiceLayer.MDRStatuses;
using PSSR.ServiceLayer.MDRStatuses.Concrete;
using PSSR.ServiceLayer.PlanServices.Concrete;
using PSSR.ServiceLayer.ProjectServices;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using PSSR.ServiceLayer.ProjectSystemServices;
using PSSR.ServiceLayer.ProjectSystemServices.Concrete;
using PSSR.ServiceLayer.SubSystemServices;
using PSSR.ServiceLayer.SubSystemServices.Concrete;
using PSSR.ServiceLayer.ValueUnits;
using PSSR.ServiceLayer.ValueUnits.Concrete;
using PSSR.ServiceLayer.WorkPackageSteps;
using PSSR.ServiceLayer.WorkPackageSteps.Concrete;

namespace PSSR.API.Controllers
{
    [ApiVersion("1.0")]
    public class ManagerProjectController : BaseManagerController
    {
        private readonly EfCoreContext _context;
        public ManagerProjectController(EfCoreContext context)
        {
            _context = context;
        }
        #region initialization items

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(WorkPackageStepListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWorkPackageStepByWorkPackage(int workPackageId)
        {
            var listService = new WorkPackageStepService(_context);

            var desciplineList = await listService.GetWorkPackageStepByWorkPackage(workPackageId);

            return new ObjectResult(desciplineList);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ServiceLayer.DesciplineServices.DesciplineListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFormsByDescipline(int desciplineId)
        {
            var listService = new ListFormDictionaryService(_context);
            var descipline = await listService.GetFormsByDescipline(desciplineId);
            return new ObjectResult(descipline);
        }
        
       
        #endregion

        #region project related

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ProjectListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProject(Guid projectId)
        {
            var projectService = new ListProjectService(_context);
            return new ObjectResult(await projectService.GetProjectDetails(projectId));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IEnumerable<ProjectListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetallCurrentUserprojects(int personId)
        {
            var projectService = new ListProjectService(_context);
            return new ObjectResult(await projectService.GetallCurrentUserprojects(personId));
        }


        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ProjectMapDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectSystems(Guid projectId)
        {
            var listService = new ListProjectSystemService(_context);
            return new ObjectResult(await listService.GetProjectSystems(projectId));
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ProjectMapDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectSubSystems(int systemId, Guid projectId)
        {
            var subSystemService = new ListProjectSubSystemService(_context);
            return new ObjectResult(await subSystemService.GetProjectSubSystems(projectId));
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ProjectSystemListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectSystemDirect(int systemId)
        {
            var listService = new ListProjectSystemService(_context);

            var system = await listService.GetSystem(systemId);
            return new ObjectResult(system);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ProjectSubSystemListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSubSystem(int id)
        {
            var listService = new ListProjectSubSystemService(_context);
            var system = await listService.GetSubSystems(id);
            return new ObjectResult(system);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ProjectSubSystemListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSubSystemBySystem(int systemId)
        {
            var listService =new ListProjectSubSystemService(_context);
            var subSystems = await listService.GetSubSystemBySystem(systemId);
            return new ObjectResult(subSystems);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(MDRStatusListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMDRStatus(int id)
        {
            var listService = new ListMDRStatusService(_context);
            var system = await listService.GetMdrStatus(id);
            return new ObjectResult(system);
        }

        #endregion

        #region plan

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IEnumerable<HirecharyPlaneDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPlanHirechary(string filterTypes,Guid projectId)
        {
            var listService = new ListPlanService(_context);
            var desciplineList = await listService.getPlanHirechary(filterTypes, projectId);

            return new ObjectResult(desciplineList);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IEnumerable<DesciplinePlanModelDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDesciplineActivityGroupList(int workPackageId, int locationId, int systemId, long subsystemId)
        {
            var listService = new ListPlanService(_context);

            var desciplineList = await listService.getActivityDescplineGroupedByWorkPackage(workPackageId, locationId, systemId, subsystemId);

            return new ObjectResult(desciplineList);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(FormDicPlanModelDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFormDictionaryGroupedByPlanDate(int workId, int locationId, int subSystemId, int desId)
        {
            var listService = new ListPlanService(_context);
            var model = await listService.GetFormDictionaryGroupedByPlanDate(workId, locationId, subSystemId, desId);
            return new ObjectResult(model);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IEnumerable<PlanActivityDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectPlanActivity(int workPackageId, int locationId, long subsystemId, int desciplineId, long formId)
        {
            var listService = new ListActivityService(_context);

            var desciplineList = await listService.GetProjectPlanActivity(workPackageId, locationId, subsystemId, desciplineId, formId);

            return new ObjectResult(desciplineList);
        }


        #endregion

    }
}