
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.DataLayer.EfCode;
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

        #region project

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IEnumerable<ProjectListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectsForAdmin()
        {
            var projectService = new ListProjectService(_context);
            return new ObjectResult(await projectService.GetProjectsForAdmin());
        }

        #endregion

    }
}
