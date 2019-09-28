
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using System.Net;
using PSSR.ServiceLayer.RoadMapServices;
using PSSR.Logic.RoadMaps;
using PSSR.Logic.LocationTypes;
using PSSR.UI.Controllers;
using Microsoft.AspNetCore.Authentication;
using PSSR.UI.Helpers.Http;
using PSSR.UI.Configuration;
using Microsoft.Extensions.Options;
using PSSR.ServiceLayer.Utils;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [ApiVersion("1.0")]
    public class WorkPackageController : BaseAdminController
    {
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;

        public WorkPackageController(IHttpClient clientService
            ,IOptions<ApplicationSettings> settings)
        {
            _clientService = clientService;
            _settings = settings;
        }

        // GET: /<controller>/
        public IActionResult GlobalData()
        {
            return View();
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(WorkPackageListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWorkPackages()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}WorkPackage/GetWorkPackages",
                authorizationToken:accessToken);

            return new ObjectResult(content);
        }
        
        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(WorkPackageListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWorkPackage([FromQuery] int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}WorkPackage/GetWorkPackage?id={id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpPost]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateWorkPackage([FromBody] ProjectWorkPackageListDto model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.PostAsync($"{_settings.Value.OilApiAddress}WorkPackage/CreateWorkPackage", model,
                authorizationToken: accessToken);
            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpPut]
        [Route("APSE/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateWorkPackage(int id, [FromBody] ProjectWorkPackageListDto model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.PutAsync($"{_settings.Value.OilApiAddress}WorkPackage/UpdateWorkPackage/{id}", model,
                authorizationToken: accessToken);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpDelete("APSE/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteWorkPackage(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.DeleteAsync($"{_settings.Value.OilApiAddress}WorkPackage/DeleteWorkPackage/{id}",
                authorizationToken: accessToken);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }
    }
}
