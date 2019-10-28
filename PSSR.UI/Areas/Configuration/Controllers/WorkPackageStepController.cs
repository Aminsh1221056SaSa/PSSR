
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Web.Http;
using PSSR.Common.CommonModels.Dtos;
using PSSR.Common.WorkPackageSteps;
using PSSR.Logic.WorkPackageSteps;
using PSSR.UI.Configuration;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.Http;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PSSR.UI.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [ApiVersion("1.0")]
    public class WorkPackageStepController: BaseAdminController
    {
        private readonly IProtectedApiClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        public WorkPackageStepController(IProtectedApiClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _clientService = clientService;
            _settings = settings;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<WorkPackageStepListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWorkPackageSteps()
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}WorkPackageStep/GetWorkPackageSteps");

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(WorkPackageStepListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWorkPackageStep(int id)
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}WorkPackageStep/GetWorkPackageStep?id={id}");

            return new ObjectResult(content);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateWorkPackageStep([FromBody] WorkPackageStepDto model)
        {
            var response = await _clientService.PostAsync($"{_settings.Value.OilApiAddress}WorkPackageStep/CreateWorkPackageStep", model);
            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateWorkPackageStep(int id, [FromBody] WorkPackageStepDto model)
        {
            var response = await _clientService.PutAsync($"{_settings.Value.OilApiAddress}WorkPackageStep/UpdateWorkPackageStep/{id}", model);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpDelete("[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteWorkPackageStep(int id)
        {
            var response = await _clientService.DeleteAsync($"{_settings.Value.OilApiAddress}WorkPackageStep/DeleteWorkPackageStep/{id}");

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }
    }
}
