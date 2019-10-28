
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.Logic.ValueUnits;
using PSSR.UI.Controllers;
using System.Collections.Generic;
using System.Net;
using PSSR.UI.Helpers.Http;
using Microsoft.Extensions.Options;
using PSSR.UI.Configuration;
using PSSR.Common.CommonModels;
using Newtonsoft.Json;
using PSSR.UI.ViewComponents;
using PSSR.Common.CommonModels.Dtos;
using PSSR.Common.ValueUnits;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [ApiVersion("1.0")]
    public class ValueUnitController : BaseAdminController
    {
        private readonly IProtectedApiClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        public ValueUnitController(IProtectedApiClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _clientService = clientService;
            _settings = settings;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<ValueUnitListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetValueUnits()
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ValueUnit/GetValueUnits");

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetValueUnitsTreeFormat()
        { 
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ValueUnit/GetValueUnitsTreeFormat");

            var model = JsonConvert.DeserializeObject<List<ValueUnitModel>>(content);
            return ViewComponent(typeof(ValuUnitTreeViewComponent), new { ValueUnits=model });
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(ValueUnitListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetValueUnit(int id)
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ValueUnit/GetValueUnit?id={id}");

            return new ObjectResult(content);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateValueUnit([FromBody] ValueUnitDto model)
        {
            var response = await _clientService.PostAsync($"{_settings.Value.OilApiAddress}ValueUnit/CreateValueUnit", model);
            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateValueUnit(int id, [FromBody] ValueUnitDto model)
        {
            var response = await _clientService.PutAsync($"{_settings.Value.OilApiAddress}ValueUnit/UpdateValueUnit/{id}", model);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpDelete("[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteValueUnit(int id)
        {
            var response = await _clientService.DeleteAsync($"{_settings.Value.OilApiAddress}ValueUnit/DeleteValueUnit/{id}");

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }
    }
}
