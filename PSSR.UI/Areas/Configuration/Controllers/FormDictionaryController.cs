
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using System.Net;
using Microsoft.AspNetCore.Http;
using PSSR.UI.Controllers;
using PSSR.UI.Helpers.Http;
using Microsoft.Extensions.Options;
using PSSR.UI.Configuration;
using PSSR.Logic.FormDictionaries;
using System.Net.Http;
using PSSR.Common.CommonModels.Dtos;
using PSSR.Common.FormDictionaryServices;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [ApiVersion("1.0")]
    public class FormDictionaryController : BaseAdminController
    {
        private readonly IProtectedApiClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        public FormDictionaryController(IProtectedApiClient clientService
            , IOptions<ApplicationSettings> settings)
        {
            _clientService = clientService;
            _settings = settings;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<FormDictionarySummaryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFormDocuments()
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}FormDocument/GetFormDocuments");

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(FormDictionarySummaryDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFormDocument(long id)
        {
            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}FormDocument/GetFormDocument?id={id}");

            return new ObjectResult(content);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateFormDocument(IFormFile File, [FromForm] string jsonString)
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(jsonString), "DocParameters");
                byte[] Bytes = new byte[File.OpenReadStream().Length + 1];
                File.OpenReadStream().Read(Bytes, 0, Bytes.Length);
                var fileContent = new ByteArrayContent(Bytes);
               
                content.Add(fileContent,"attachment",File.FileName);
                var response = await _clientService.PostAsyncV1($"{_settings.Value.OilApiAddress}FormDocument/CreateFormDocument", content);
                var mcontent = await response.Content.ReadAsStringAsync();
                return new ObjectResult(mcontent);
            }
        }

        [HttpPut]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateFormDocument(long id, [FromBody] FormDictionaryDto model)
        {
            var response = await _clientService.PutAsync($"{_settings.Value.OilApiAddress}FormDocument/UpdateFormDocument/{id}", model);

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

        [HttpDelete("[action]/{id}")]
        [ProducesResponseType(typeof(ResultResponseDto<string, int>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteFormDocument(long id)
        {
            var response = await _clientService.DeleteAsync($"{_settings.Value.OilApiAddress}FormDocument/DeleteFormDocument/{id}");

            var content = await response.Content.ReadAsStringAsync();
            return new ObjectResult(content);
        }

    }
}
