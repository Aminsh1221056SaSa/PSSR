
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ContractorServices;
using PSSR.ServiceLayer.ContractorServices.Concrete;
using PSSR.ServiceLayer.PersonService.Concrete;
using PSSR.ServiceLayer.ProjectServices;
using PSSR.ServiceLayer.ProjectServices.Concrete;

namespace PSSR.API.Controllers
{
    [ApiVersion("1.0")]
    public class AdminController : BaseAdminController
    {
        private readonly EfCoreContext _context;
        public AdminController(EfCoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IEnumerable<ProjectListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectsForAdmin()
        {
            var projectService = new ListProjectService(_context);
            return new ObjectResult(await projectService.GetProjectsForAdmin());
        }

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
        [ProducesResponseType(typeof(ContractorListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetContractor(int contractorid)
        {
            var contractorService = new ListContractorService(_context);
            return new ObjectResult(await contractorService.GetCurrentContractors(contractorid));
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ServiceLayer.PersonService.PersonListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPerson(int personId)
        {
            var listService = new ListPersonSystemService(_context);
            var person = await listService.GetPerson(personId);
            return new ObjectResult(person);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IEnumerable<ServiceLayer.PersonService.PersonSummaryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPersons()
        {
            var listService = new ListPersonSystemService(_context);
            var person = await listService.GetPersons();
            return new ObjectResult(person);
        }
    }
}
