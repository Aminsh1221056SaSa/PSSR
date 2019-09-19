using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BskaGenericCoreLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.API.Helper;
using PSSR.API.Models;
using PSSR.DataLayer.EfClasses.Person;
using PSSR.DataLayer.EfCode;
using PSSR.Logic.Contractors;
using PSSR.ServiceLayer.ContractorServices;
using PSSR.ServiceLayer.ContractorServices.Concrete;
using PSSR.ServiceLayer.Utils;

namespace PSSR.API.Controllers.Admins
{
    [ApiVersion("1.0")]
    public class ContractorController : BaseAdminController
    {
        private readonly EfCoreContext _context;
        public ContractorController(EfCoreContext context)
        {
            _context = context;
        }
        #region contractors

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ContractorListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetContractors()
        {
            var contractorService = new ListContractorService(_context);
            return new ObjectResult(await contractorService.GetAllContractors());
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(ContractorListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetContractor(int id)
        {
            var contractorService = new ListContractorService(_context);
            return new ObjectResult(await contractorService.GetCurrentContractors(id));
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateContractor(ContractorDto model,
            [FromServices]IActionService<IPlaceContractorAction> service)
        {
            var contractor = service.RunBizAction<Contractor>(model);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String,int> { Key = HttpStatusCode.OK, Value = "Contractor created.." ,Subject=contractor.Id});
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String,int> { Key = HttpStatusCode.BadRequest, Value = errors });
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public IActionResult UpdateContractor(int id, ContractorDto model,
            [FromServices]IActionService<IUpdateContractorAction> service)
        {
            model.Id = id;
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String,int> { Key = HttpStatusCode.OK, Value = "Contractor updated.." });
            }

           var errors= service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String,int> { Key=HttpStatusCode.BadRequest,Value=errors,Subject=model.Id});
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteContractor(int id,[FromServices]IActionService<IDeleteContractorAction> service)
        {
            service.RunBizAction(id);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.OK, Value = "Contractor Deleted.." });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors, Subject = id });
        }
        #endregion
    }
}