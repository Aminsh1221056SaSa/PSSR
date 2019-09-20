
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Web.Http;
using PSSR.Logic.Persons;
using PSSR.ServiceLayer.PersonService;
using PSSR.ServiceLayer.Utils;
using PSSR.UI.Configuration;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.Http;

namespace PSSR.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiVersion("1.0")]
    public class PersonController : BaseAdminController
    {
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;

        public PersonController(IHttpClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _clientService = clientService;
            _settings = settings;
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(List<PersonSummaryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPersons()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Person/GetPersons",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(PersonListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPerson(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Person/GetPerson/{id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpPost]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreatePerson([FromBody] PersonDto model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.PostAsync($"{_settings.Value.OilApiAddress}Person/CreatePerson", model,
                authorizationToken: accessToken);
            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpPut]
        [Route("APSE/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] PersonDto model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.PutAsync($"{_settings.Value.OilApiAddress}Person/UpdatePerson/{id}", model,
                authorizationToken: accessToken);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpDelete("APSE/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.DeleteAsync($"{_settings.Value.OilApiAddress}Person/DeletePerson/{id}",
                authorizationToken: accessToken);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }
    }
}