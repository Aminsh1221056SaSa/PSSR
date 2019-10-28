
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSSR.Logic.Contractors;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.Http;
using PSSR.UI.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Web.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiVersion("1.0")]
    public class ContractorController : BaseAdminController
    {
        private readonly IProtectedApiClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;

        public ContractorController(IProtectedApiClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _clientService = clientService;
            _settings = settings;
        }
        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetContractors()
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Contractor/GetContractors");

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> GetContractor(int id)
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}Contractor/GetContractor/{id}");

            return new ObjectResult(content);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateContractor([FromBody] ContractorDto model)
        {
            var response = await _clientService.PostAsync($"{_settings.Value.OilApiAddress}Contractor/CreateContractor",model);
            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<IActionResult> UpdateContractor(int id,[FromBody] ContractorDto model)
        {
            var response = await _clientService.PutAsync($"{_settings.Value.OilApiAddress}Contractor/UpdateContractor/{id}", model);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteContractor(int id)
        {
            var response = await _clientService.DeleteAsync($"{_settings.Value.OilApiAddress}Contractor/DeleteContractor/{id}");

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }
    }
}
