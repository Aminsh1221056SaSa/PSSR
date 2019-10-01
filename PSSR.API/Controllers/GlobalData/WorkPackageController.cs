using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BskaGenericCoreLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.API.Helper;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;
using PSSR.Logic.RoadMaps;
using PSSR.Logic.WorkPackages;
using PSSR.ServiceLayer.RoadMapServices;
using PSSR.ServiceLayer.RoadMapServices.Concrete;
using PSSR.ServiceLayer.Utils;

namespace PSSR.API.Controllers.GlobalData
{
    [ApiVersion("1.0")]
    public class WorkPackageController : BaseAdminController
    {
        private readonly EfCoreContext _context;
        public WorkPackageController(EfCoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<WorkPackageListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWorkPackages()
        {
            var roadMapService = new ListWorkPackageService(_context);
            return new ObjectResult(await roadMapService.GetRoadMapsAsync());
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(WorkPackageListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWorkPackage(int id)
        {
            var roadMapService = new ListWorkPackageService(_context);
            return new ObjectResult(await roadMapService.GetRoadMapAsycn(id));
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateWorkPackage([FromBody] ProjectWorkPackageListDto model
            , [FromServices]IActionService<IPlaceWorkPackageAction> service)
        {
            var roadMap = service.RunBizAction<WorkPackage>(model);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int>
                {
                    Key = HttpStatusCode.OK,
                    Value = "WorkPackage Created...",
                    Subject =roadMap.Id
                });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors });
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public IActionResult UpdateWorkPackage(int id, [FromBody] ProjectWorkPackageListDto model
           , [FromServices]IActionService<IUpdateRoadMapAction> service)
        {
            model.Id = id;
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.OK, Value = "WorkPackage updated..", Subject = model.Id });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);

            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors, Subject = model.Id });
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteWorkPackage(int id, [FromServices]IActionService<IDeleteWorkPackageAction> service)
        {
            service.RunBizAction(id);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.OK, Value = "delete workpackage success!!" });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors });
        }

    }
}