
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using System.Net;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.Http;
using Microsoft.Extensions.Options;
using PSSR.UI.Configuration;
using PSSR.Logic.Desciplines;
using PSSR.Common.CommonModels.Dtos;
using PSSR.Common.DesciplineServices;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [ApiVersion("1.0")]
    public class DesciplineController : BaseAdminController
    {
        private readonly IProtectedApiClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        public DesciplineController(IProtectedApiClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _clientService = clientService;
            _settings = settings;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<DesciplineListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDesciplines()
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Descipline/GetDesciplines");

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(DesciplineListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDescipline(int id)
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Descipline/GetDescipline?id={id}");

            return new ObjectResult(content);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateDescipline([FromBody] DesciplineDto model)
        {
            var response = await _clientService.PostAsync($"{_settings.Value.OilApiAddress}Descipline/CreateDescipline", model);
            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateDescipline(int id, [FromBody] DesciplineDto model)
        {
            var response = await _clientService.PutAsync($"{_settings.Value.OilApiAddress}Descipline/UpdateDescipline/{id}", model);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpDelete("[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteDescipline(int id)
        {
            var response = await _clientService.DeleteAsync($"{_settings.Value.OilApiAddress}Descipline/DeleteDescipline/{id}");

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

    }
}
