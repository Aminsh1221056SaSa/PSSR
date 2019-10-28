using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BskaGenericCoreLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.API.Helper;
using PSSR.Common.CommonModels.Dtos;
using PSSR.Common.PersonService;
using PSSR.DataLayer.EfClasses.Person;
using PSSR.DataLayer.EfCode;
using PSSR.Logic.Persons;
using PSSR.ServiceLayer.PersonService.Concrete;

namespace PSSR.API.Controllers.Admins
{
    [ApiVersion("1.0")]
    public class PersonController : BaseAdminController
    {
        private readonly EfCoreContext _context;
        public PersonController(EfCoreContext context)
        {
            _context = context;
        }
        #region Persons

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<PersonSummaryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPersons()
        {
            var personService = new ListPersonSystemService(_context);
            return new ObjectResult(await personService.GetPersons());
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(PersonListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPerson(int id)
        {
            var personService = new ListPersonSystemService(_context);
            return new ObjectResult(await personService.GetPerson(id));
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CreatePerson(PersonDto model,
            [FromServices]IActionService<IPlacePersonAction> service)
        {
            var person = service.RunBizAction<Person>(model);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String,int> { Key = HttpStatusCode.OK, Value = "person created..", Subject= person.Id});
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String,int> { Key = HttpStatusCode.BadRequest, Value = errors });
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public IActionResult UpdatePerson(int id, PersonDto model,
            [FromServices]IActionService<IUpdatePersonAction> service)
        {
            model.Id = id;
            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String,int> { Key = HttpStatusCode.OK, Value = "person updated.." });
            }

           var errors= service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String,int> { Key=HttpStatusCode.BadRequest,Value=errors,Subject=model.Id});
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult DeletePerson(int id,[FromServices]IActionService<IDeletePersonAction> service)
        {
            service.RunBizAction(id);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.OK, Value = "Person Deleted.." });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors, Subject = id });
        }
        #endregion
    }
}