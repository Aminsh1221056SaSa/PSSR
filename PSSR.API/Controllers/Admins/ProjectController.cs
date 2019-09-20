using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BskaGenericCoreLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.API.Helper;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfCode;
using PSSR.Logic.Projects;
using PSSR.ServiceLayer.PersonService;
using PSSR.ServiceLayer.ProjectServices;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using PSSR.ServiceLayer.Utils;

namespace PSSR.API.Controllers.Admins
{
    [ApiVersion("1.0")]
    public class ProjectController : BaseAdminController
    {
        private readonly EfCoreContext _context;
        public ProjectController(EfCoreContext context)
        {
            _context = context;
        }
        #region Project

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<ProjectSummaryListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjects()
        {
            var projectService = new ListProjectService(_context);
            return new ObjectResult(await projectService.GetProjectSummary());
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(ProjectListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProject(Guid id)
        {
            var projectService = new ListProjectService(_context);
            return new ObjectResult(await projectService.GetProjectDetails(id));
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateProject(ProjectDto model,
            [FromServices]IActionService<IPlaceProjectAction> service)
        {
            var project = service.RunBizAction<Project>(model);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String,Guid> { Key = HttpStatusCode.OK, Value = "project created..", Subject= project.Id});
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String,Guid> { Key = HttpStatusCode.BadRequest, Value = errors });
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public IActionResult UpdateProject(Guid id, ProjectDto model,
            [FromServices]IActionService<IUpdateProjectAction> service)
        {
            model.Id = id;
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String,Guid> { Key = HttpStatusCode.OK, Value = "project updated.." });
            }

           var errors= service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String,Guid> { Key=HttpStatusCode.BadRequest,Value=errors,Subject=model.Id});
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteProject(Guid id,[FromServices]IActionService<IDeleteProjectAction> service)
        {
            service.RunBizAction(id);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, Guid> { Key = HttpStatusCode.OK, Value = "Project Deleted.." });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String, Guid> { Key = HttpStatusCode.BadRequest, Value = errors, Subject = id });
        }
        #endregion
    }
}