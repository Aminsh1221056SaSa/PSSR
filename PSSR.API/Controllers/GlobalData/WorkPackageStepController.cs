
using BskaGenericCoreLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.API.Helper;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;
using PSSR.Logic.WorkPackageSteps;
using PSSR.ServiceLayer.Utils;
using PSSR.ServiceLayer.WorkPackageSteps;
using PSSR.ServiceLayer.WorkPackageSteps.Concrete;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PSSR.API.Controllers.GlobalData
{
    [ApiVersion("1.0")]
    public class WorkPackageStepController : BaseAdminController
    {
        private readonly EfCoreContext _context;
        public WorkPackageStepController(EfCoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<WorkPackageStepListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWorkPackageSteps()
        {
            var listService = new WorkPackageStepService(_context);

            var desciplineList = await listService.GetWorkPackageSteps();

            return new ObjectResult(desciplineList);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(WorkPackageStepListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWorkPackageStep(int id)
        {
            var listService = new WorkPackageStepService(_context);

            var desciplineList = await listService.GetWorkPackageStep(id);

            return new ObjectResult(desciplineList);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateWorkPackageStep(WorkPackageStepDto model,
          [FromServices]IActionService<IPlaceWorkStepPackageAction> service)
        {
            var wStep = service.RunBizAction<WorkPackageStep>(model);
            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int>
                {
                    Key = HttpStatusCode.OK,
                    Value = "WorkPackageStep Created...",
                    Subject = wStep.Id
                });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors });
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public IActionResult UpdateWorkPackageStep(int id, [FromBody] WorkPackageStepDto model,
            [FromServices]IActionService<IUpdateWorkPackageStepAction> service)
        {
            model.Id = id;
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int>
                {
                    Key = HttpStatusCode.OK,
                    Value = "WorkPackageStep updated.."
                });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);

            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors, Subject = model.Id });
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteWorkPackageStep(int id, [FromServices]IActionService<IDeleteWorkPackageStepAction> service)
        {
            service.RunBizAction(id);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int>
                {
                    Key = HttpStatusCode.OK,
                    Value = "delete workpackagestep success!!"
                });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors });
        }
    }
}