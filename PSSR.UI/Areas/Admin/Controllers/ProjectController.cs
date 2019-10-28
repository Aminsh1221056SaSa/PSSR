
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSSR.Logic.Projects;
using Microsoft.Web.Http;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.Http;
using Microsoft.Extensions.Options;
using PSSR.UI.Configuration;
using System;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiVersion("1.0")]
    public class ProjectController : BaseAdminController
    {
        private readonly IProtectedApiClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        public ProjectController( IProtectedApiClient clientService
            ,IOptions<ApplicationSettings> settings)
        {
            _clientService = clientService;
            _settings = settings;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetProjects()
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Project/GetProjects");

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> GetProject(Guid id)
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Project/GetProject/{id}");

            return new ObjectResult(content);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDto model)
        {
            var response = await _clientService.PostAsync($"{_settings.Value.OilApiAddress}Project/CreateProject", model);
            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] ProjectDto model)
        {
            var response = await _clientService.PutAsync($"{_settings.Value.OilApiAddress}Project/UpdateProject/{id}", model);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var response = await _clientService.DeleteAsync($"{_settings.Value.OilApiAddress}Project/DeleteProject/{id}");

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }
    }
}
