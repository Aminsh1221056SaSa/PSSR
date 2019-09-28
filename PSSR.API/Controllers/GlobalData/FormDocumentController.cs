using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BskaGenericCoreLib;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.API.Helper;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;
using PSSR.Logic.FormDictionaries;
using PSSR.ServiceLayer.FormDictionaryServices;
using PSSR.ServiceLayer.FormDictionaryServices.Concrete;
using PSSR.ServiceLayer.Utils;

namespace PSSR.API.Controllers.GlobalData
{
    [ApiVersion("1.0")]
    public class FormDocumentController : BaseAdminController
    {
        private readonly EfCoreContext _context;
        private readonly IHostingEnvironment _enviroment;

        public FormDocumentController(EfCoreContext context, IHostingEnvironment enviroment)
        {
            this._context = context;
            this._enviroment = enviroment;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<FormDictionarySummaryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFormDocuments()
        {
            var listService = new ListFormDictionaryService(_context);

            var desciplineList = await listService.GetformDictionaryies();

            return new ObjectResult(desciplineList);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(FormDictionaryListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFormDocument(long id)
        {
            var listService = new ListFormDictionaryService(_context);

            var desciplineList = await listService.GetformDictionary(id);

            return new ObjectResult(desciplineList);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateFormDocument(FormDictionaryDto model,
           [FromServices]IActionService<IPlaceFormDictionaryAction> service)
        {
            if (model.File == null)
            {
                service.Status.AddError("File Not Valid!!!", "Form Document");
            }

            var formService = new ListFormDictionaryService(_context);
            if (await formService.HasDuplicatedCode(model.Code))
            {
                service.Status.AddError("Entered code is taked from other form!!!", "Form Document");
            }

            if (!service.Status.HasErrors)
            {
                if (model.File != null && model.File.Length > 0)
                {
                    FormDocumentFileHelper docHelper = new FormDocumentFileHelper();
                    string filePath = await docHelper.SaveFormDocument(model.Code, model.File, _enviroment);
                    model.FileName = Path.Combine(Path.Combine(filePath, $"{model.Code}"));
                }

                var formDic = service.RunBizAction<FormDictionary>(model);
                return new ObjectResult(new ResultResponseDto<String, long>
                {
                    Key = HttpStatusCode.OK,
                    Value = "FormDocument created..",
                    Subject=formDic.Id
                });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors });
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<IActionResult> UpdateFormDocument(long id, FormDictionaryDto model,
           [FromServices]IActionService<IUpdateFormDictionaryAction> service)
        {
            model.Id = id;
            if (model.File != null && model.File.Length > 0)
            {
                FormDocumentFileHelper docHelper = new FormDocumentFileHelper();
                string filePath = await docHelper.SaveFormDocument(model.Code, model.File, _enviroment);
                model.FileName = Path.Combine(Path.Combine(filePath, $"{model.Code}"));
            }

            service.RunBizAction(model);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int>
                {
                    Key = HttpStatusCode.OK,
                    Value = "FormDocument updated.."
                });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);

            return new ObjectResult(new ResultResponseDto<String, long> { Key = HttpStatusCode.BadRequest, Value = errors, Subject = model.Id });
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteFormDocument(int id, [FromServices]IActionService<IFormDictionaryDeleteAction> service)
        {
            service.RunBizAction(id);

            if (!service.Status.HasErrors)
            {
                return new ObjectResult(new ResultResponseDto<String, int>
                {
                    Key = HttpStatusCode.OK,
                    Value = "delete form document success!!"
                });
            }

            var errors = service.Status.CopyErrorsToString(ModelState);
            return new ObjectResult(new ResultResponseDto<String, int> { Key = HttpStatusCode.BadRequest, Value = errors });
        }
    }
}