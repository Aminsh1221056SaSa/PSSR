
using System;
using System.Net;
using System.Threading.Tasks;
using BskaGenericCoreLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.API.Helper;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;
using PSSR.Logic.LocationTypes;
using PSSR.ServiceLayer.RoadMapServices;
using PSSR.ServiceLayer.RoadMapServices.Concrete;
using PSSR.ServiceLayer.Utils;

namespace PSSR.API.Controllers.GlobalData
{
    [ApiVersion("1.0")]
    public class LocationController : BaseAdminController
    {
        private readonly EfCoreContext _context;
        public LocationController(EfCoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(LocationListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLocations()
        {
            var roadMapService = new ListWorkPackageService(_context);
            return new ObjectResult(await roadMapService.GetLocationsAsync());
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(LocationListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLocation(int id)
        {
            var roadMapService = new ListWorkPackageService(_context);
            return new ObjectResult(await roadMapService.GetLocationAsycn(id));
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateLocation([FromBody] LocationTypeDto model
            ,[FromServices]IActionService<IPlaceLocationTypeAction> service)
        {
            var roadMap = service.RunBizAction<LocationType>(model);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int>
                {
                    Key = HttpStatusCode.OK,
                    Value = "WorkPackage Created...",
                    Subject = roadMap.Id
                });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors });
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public IActionResult UpdateLocation(int id, [FromBody] LocationTypeDto model
          , [FromServices]IActionService<IUpdateLocationTypeAction> service)
        {
            model.Id = id;
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.OK,
                    Value = "Location updated..",
                    Subject = model.Id
                });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);

            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors, Subject = model.Id });
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteLocation(int id, [FromServices]IActionService<ILocationDeleteAction> service)
        {
            service.RunBizAction(id);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.OK,
                    Value = "delete location success!!" });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors });
        }
    }
}