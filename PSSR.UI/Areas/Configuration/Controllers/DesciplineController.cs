
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using System.Net;
using PSSR.ServiceLayer.DesciplineServices;
using PSSR.ServiceLayer.Utils;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.Http;
using Microsoft.Extensions.Options;
using PSSR.UI.Configuration;
using Microsoft.AspNetCore.Authentication;
using PSSR.Logic.Desciplines;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [ApiVersion("1.0")]
    public class DesciplineController : BaseAdminController
    {
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        public DesciplineController(IHttpClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _clientService = clientService;
            _settings = settings;
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(List<DesciplineListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDesciplines()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Descipline/GetDesciplines",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(DesciplineListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDescipline([FromQuery] int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Descipline/GetDescipline?id={id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpPost]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateDescipline([FromBody] DesciplineDto model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.PostAsync($"{_settings.Value.OilApiAddress}Descipline/CreateDescipline", model,
                authorizationToken: accessToken);
            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpPut]
        [Route("APSE/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateDescipline(int id, [FromBody] DesciplineDto model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.PutAsync($"{_settings.Value.OilApiAddress}Descipline/UpdateDescipline/{id}", model,
                authorizationToken: accessToken);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpDelete("APSE/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteDescipline(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.DeleteAsync($"{_settings.Value.OilApiAddress}Descipline/DeleteDescipline/{id}",
                authorizationToken: accessToken);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

    }
}
