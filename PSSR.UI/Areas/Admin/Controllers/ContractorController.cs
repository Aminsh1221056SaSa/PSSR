
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSSR.Logic.Contractors;
using PSSR.ServiceLayer.ContractorServices;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.Http;
using PSSR.UI.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Web.Http;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using PSSR.ServiceLayer.Utils;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiVersion("1.0")]
    public class ContractorController : BaseAdminController
    {
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;

        public ContractorController(IHttpClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _clientService = clientService;
            _settings = settings;
        }

        [Authorize(Policy = "dataEventRecordsAdmin")]
        // GET: /<controller>/
        public IActionResult Contractor()
        {
            return View();
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(List<ContractorListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetContractors()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Contractor/GetContractors",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(ContractorListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetContractor(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Contractor/GetContractor/{id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpPost]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(ResultResponseDto<string,int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateContractor([FromBody] ContractorDto model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.PostAsync($"{_settings.Value.OilApiAddress}Contractor/CreateContractor",model,
                authorizationToken: accessToken);
            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpPut]
        [Route("APSE/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string,int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateContractor(int id,[FromBody] ContractorDto model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.PutAsync($"{_settings.Value.OilApiAddress}Contractor/UpdateContractor/{id}", model,
                authorizationToken: accessToken);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpDelete("APSE/[controller]/[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteContractor(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _clientService.DeleteAsync($"{_settings.Value.OilApiAddress}Contractor/DeleteContractor/{id}",
                authorizationToken: accessToken);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }
    }
}
