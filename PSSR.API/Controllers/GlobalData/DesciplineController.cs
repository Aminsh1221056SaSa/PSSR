using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BskaGenericCoreLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.API.Helper;
using PSSR.Common.CommonModels.Dtos;
using PSSR.Common.DesciplineServices;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;
using PSSR.Logic.Desciplines;
using PSSR.ServiceLayer.DesciplineServices.Concrete;

namespace PSSR.API.Controllers.GlobalData
{
    [ApiVersion("2.0")]
    public class DesciplineController : BaseAdminController
    {
        private readonly EfCoreContext _context;
        public DesciplineController(EfCoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<DesciplineListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDesciplines()
        {
            var listService = new ListDesciplineService(_context);

            var desciplineList = await listService.GetAllDesciplines();

            return new ObjectResult(desciplineList);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DesciplineListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDescipline(int id)
        {
            var listService = new ListDesciplineService(_context);

            var desciplineList = await listService.GetDescipline(id);

            return new ObjectResult(desciplineList);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateDescipline(DesciplineDto model,
           [FromServices]IActionService<IPlaceDesciplineAction> service)
        {
            var dto = new PlaceDesciplineDto(model);
            var descipline = service.RunBizAction<Descipline>(dto);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int>
                {
                    Key = HttpStatusCode.OK,
                    Value = "Descipline Created...",
                    Subject = descipline.Id
                });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors });
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public IActionResult UpdateDescipline(int id, [FromBody] DesciplineDto model,
           [FromServices]IActionService<IUpdateDesciplineAction> service)
        {
            model.Id = id;
            var dto = new PlaceDesciplineDto(model);

            service.RunBizAction(dto);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int>
                {
                    Key = HttpStatusCode.OK,
                    Value = "Descipline updated..",
                    Subject = model.Id
                });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);

            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors, Subject = model.Id });
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteDescipline(int id, [FromServices]IActionService<IDesciplineDeleteAction> service)
        {
            service.RunBizAction(id);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int>
                {
                    Key = HttpStatusCode.OK,
                    Value = "delete descipline success!!"
                });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors });
        }
    }
}