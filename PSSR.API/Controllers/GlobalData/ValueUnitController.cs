using BskaGenericCoreLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.API.Helper;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;
using PSSR.Logic.ValueUnits;
using PSSR.ServiceLayer.Utils;
using PSSR.ServiceLayer.ValueUnits;
using PSSR.ServiceLayer.ValueUnits.Concrete;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PSSR.API.Controllers.GlobalData
{
    [ApiVersion("1.0")]
    public class ValueUnitController : BaseAdminController
    {
        private readonly EfCoreContext _context;

        public ValueUnitController(EfCoreContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<ValueUnitListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetValueUnits()
        {
            var listService = new ListValueUnitService(_context);

            var valueUnits = await listService.GetValueUnitDtos();

            return new ObjectResult(valueUnits);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ValueUnitListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetValueUnit(int id)
        {
            var listService = new ListValueUnitService(_context);

            var valueUnits = await listService.GetValueUnit(id);

            return new ObjectResult(valueUnits);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateValueUnit(ValueUnitDto model,
           [FromServices]IActionService<IPlaceValuUnitAction> service)
        {
            var valueUnit = service.RunBizAction<ValueUnit>(model);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int>
                {
                    Key = HttpStatusCode.OK,
                    Value = "value unit created..",
                    Subject = valueUnit.Id
                });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors });
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public IActionResult UpdateValueUnit(int id, ValueUnitDto model,
          [FromServices]IActionService<IUpdateValueUnitAction> service)
        {
            model.Id = id;
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int>
                {
                    Key = HttpStatusCode.OK,
                    Value = "value unit updated.."
                });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);

            return new ObjectResult(new ResultResponseDto<String, long> { Key = HttpStatusCode.BadRequest,
                Value = errors, Subject = model.Id });
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteValueUnit(int id, [FromServices]IActionService<IDeleteValueUnitAction> service)
        {
            service.RunBizAction(id);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int>
                {
                    Key = HttpStatusCode.OK,
                    Value = "delete value unit success!!"
                });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors });
        }
    }
}
