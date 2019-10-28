
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Web.Http;
using PSSR.Logic.Persons;
using PSSR.UI.Configuration;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.Http;

namespace PSSR.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiVersion("1.0")]
    public class PersonController : BaseAdminController
    {
        private readonly IProtectedApiClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;

        public PersonController(IProtectedApiClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _clientService = clientService;
            _settings = settings;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetPersons()
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Person/GetPersons");

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> GetPerson(int id)
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Person/GetPerson/{id}");

            return new ObjectResult(content);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreatePerson([FromBody] PersonDto model)
        {
            var response = await _clientService.PostAsync($"{_settings.Value.OilApiAddress}Person/CreatePerson", model);
            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] PersonDto model)
        {
            var response = await _clientService.PutAsync($"{_settings.Value.OilApiAddress}Person/UpdatePerson/{id}", model);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var response = await _clientService.DeleteAsync($"{_settings.Value.OilApiAddress}Person/DeletePerson/{id}");

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }
    }
}