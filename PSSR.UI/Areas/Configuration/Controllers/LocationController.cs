
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Web.Http;
using PSSR.Common.CommonModels.Dtos;
using PSSR.Common.RoadMapServices;
using PSSR.Logic.LocationTypes;
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
        private readonly IProtectedApiClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;

        public LocationController(IProtectedApiClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _clientService = clientService;
            _settings = settings;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(WorkPackageListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLocations()
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Location/GetLocations");

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(WorkPackageListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLocation(int id)
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Location/GetLocation?id={id}");

            return new ObjectResult(content);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateLocation([FromBody] LocationTypeDto model)
        {
            var response = await _clientService.PostAsync($"{_settings.Value.OilApiAddress}Location/CreateLocation", model);
            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] LocationTypeDto model)
        {
            var response = await _clientService.PutAsync($"{_settings.Value.OilApiAddress}Location/UpdateLocation/{id}", model);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpDelete("[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var response = await _clientService.DeleteAsync($"{_settings.Value.OilApiAddress}Location/DeleteLocation/{id}");

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }
    }
}