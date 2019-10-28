
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using System.Net;
using PSSR.Logic.RoadMaps;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.Http;
using PSSR.UI.Configuration;
using Microsoft.Extensions.Options;
using PSSR.Common.CommonModels.Dtos;
using PSSR.Common.RoadMapServices;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [ApiVersion("1.0")]
    public class WorkPackageController : BaseAdminController
    {
        private readonly IProtectedApiClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;

        public WorkPackageController(IProtectedApiClient clientService
            ,IOptions<ApplicationSettings> settings)
        {
            _clientService = clientService;
            _settings = settings;
        }

        // GET: /<controller>/
       
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(WorkPackageListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWorkPackages()
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}WorkPackage/GetWorkPackages");

            return new ObjectResult(content);
        }
        
        [HttpGet]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(WorkPackageListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWorkPackage(int id)
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}WorkPackage/GetWorkPackage?id={id}");

            return new ObjectResult(content);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateWorkPackage([FromBody] ProjectWorkPackageListDto model)
        {
            var response = await _clientService.PostAsync($"{_settings.Value.OilApiAddress}WorkPackage/CreateWorkPackage", model);
            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateWorkPackage(int id, [FromBody] ProjectWorkPackageListDto model)
        {
            var response = await _clientService.PutAsync($"{_settings.Value.OilApiAddress}WorkPackage/UpdateWorkPackage/{id}", model);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpDelete("[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteWorkPackage(int id)
        {
            var response = await _clientService.DeleteAsync($"{_settings.Value.OilApiAddress}WorkPackage/DeleteWorkPackage/{id}");

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }
    }
}
