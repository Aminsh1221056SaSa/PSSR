
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using System.Net;
using PSSR.ServiceLayer.Utils;
using Microsoft.AspNetCore.Http;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.Http;
using Microsoft.Extensions.Options;
using PSSR.UI.Configuration;
using Microsoft.AspNetCore.Authentication;
using PSSR.ServiceLayer.FormDictionaryServices;
using PSSR.Logic.FormDictionaries;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [ApiVersion("1.0")]
    public class FormDictionaryController : BaseAdminController
    {
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        public FormDictionaryController(IHttpClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _clientService = clientService;
            _settings = settings;
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(List<FormDictionarySummaryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFormDocuments()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}FormDocument/GetFormDocuments",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(FormDictionarySummaryDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFormDocument([FromQuery] long id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}FormDocument/GetFormDocument?id={id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpPost]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateFormDocument([FromBody] FormDictionaryDto model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.PostAsync($"{_settings.Value.OilApiAddress}FormDocument/CreateFormDocument", model,
                authorizationToken: accessToken);
            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpPut]
        [Route("APSE/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateFormDocument(long id, [FromBody] FormDictionaryDto model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.PutAsync($"{_settings.Value.OilApiAddress}FormDocument/UpdateFormDocument/{id}", model,
                authorizationToken: accessToken);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpDelete("APSE/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteFormDocument(long id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.DeleteAsync($"{_settings.Value.OilApiAddress}FormDocument/DeleteFormDocument/{id}",
                authorizationToken: accessToken);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

    }
}
