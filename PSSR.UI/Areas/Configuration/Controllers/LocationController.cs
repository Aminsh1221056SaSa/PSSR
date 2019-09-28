
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Web.Http;
using PSSR.Logic.LocationTypes;
using PSSR.ServiceLayer.RoadMapServices;
using PSSR.ServiceLayer.Utils;
using PSSR.UI.Configuration;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.Http;
using System.Net;
using System.Threading.Tasks;

namespace PSSR.UI.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [ApiVersion("1.0")]
    public class LocationController : BaseAdminController
    {
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;

        public LocationController(IHttpClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _clientService = clientService;
            _settings = settings;
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(WorkPackageListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLocations()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Location/GetLocations",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(WorkPackageListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLocation([FromQuery] int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Location/GetLocation?id={id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpPost]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateLocation([FromBody] LocationTypeDto model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.PostAsync($"{_settings.Value.OilApiAddress}Location/CreateLocation", model,
                authorizationToken: accessToken);
            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpPut]
        [Route("APSE/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] LocationTypeDto model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.PutAsync($"{_settings.Value.OilApiAddress}Location/UpdateLocation/{id}", model,
                authorizationToken: accessToken);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpDelete("APSE/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.DeleteAsync($"{_settings.Value.OilApiAddress}Location/DeleteLocation/{id}",
                authorizationToken: accessToken);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }
    }
}