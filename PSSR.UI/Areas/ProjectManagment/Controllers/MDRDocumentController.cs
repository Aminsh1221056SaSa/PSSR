using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.DataLayer.EfCode;
using Microsoft.AspNetCore.Hosting;
using PSSR.ServiceLayer.MDRDocumentServices.Concrete;
using PSSR.ServiceLayer.MDRDocumentServices;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using PSSR.ServiceLayer.Logger;
using PSSR.ServiceLayer.Utils;
using BskaGenericCoreLib;
using PSSR.Logic.MDRDocuments;
using PSSR.UI.Helpers;
using AutoMapper;
using System.Net;
using PSSR.UI.Models;
using PSSR.ServiceLayer.RoadMapServices.Concrete;
using PSSR.UI.Controllers;
using Microsoft.AspNetCore.Authentication;
using PSSR.UI.Helpers.Http;
using PSSR.UI.Configuration;
using Microsoft.Extensions.Options;
using PSSR.UI.Helpers.CashHelper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using PSSR.UI.Helpers.Security;
using Microsoft.AspNetCore.Authorization;
using PSSR.DataLayer.EfClasses.Projects.MDRS;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSSR.UI.Areas.ProjectManagment.Controllers
{
    [Area("ProjectManagment")]
    [ApiVersion("1.0")]
    public class MDRDocumentController : BaseManagerController
    {
        private readonly EfCoreContext _context;
        private readonly IHostingEnvironment _enviroment;
        private readonly IMapper _mapper;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        private readonly IMasterDataCacheOperations _masterDataCache;

        public MDRDocumentController(EfCoreContext context, IHostingEnvironment enviroment, IMapper mapper, IHttpClient clientService
            , IOptions<ApplicationSettings> settings, IMasterDataCacheOperations masterDataCache)
        {
            _context = context;
            this._enviroment = enviroment;
            this._mapper = mapper;
            _clientService = clientService;
            _settings = settings;
            _masterDataCache = masterDataCache;
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        public async Task<IActionResult> MDRDocument(MDRDocumentSortFilterPageOptions options)
        {
            var listService =new ListMDRDocumentService(_context);

            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            var mdrList = (await listService.SortFilterPage(options,project.Id)).ToList();

            return View(new MDRDocumentListCombinedDto(options, mdrList));
        }

        [HttpGet]
        public async Task<IActionResult> CreateMDRDocument()
        {
            var roadMapService = new ListWorkPackageService(_context);
            var items = await roadMapService.GetRoadMapsAsync();
            ViewBag.ProjectWBSs = items;
            return View(new MDRDocumentListDto());
        }

        [HttpGet]
        public async Task<IActionResult> EditMDRDocument(long id)
        {
            var roadMapService = new ListWorkPackageService(_context);
            ViewBag.ProjectWBSs = await roadMapService.GetRoadMapsAsync();

            var listService = new ListMDRDocumentService(_context);

            var mdrList = await listService.GetMdrDocument(id);
            return View(mdrList);
        }

        [HttpGet]
        public IActionResult MDRDocumentDetails(long id)
        {
            return View(id);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(MDRDocumentListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetListSummary([FromQuery] string filterByOption, [FromQuery] string sortByOption
           , [FromQuery] string filterValue, [FromQuery]int pageNum, [FromQuery] string query = "")
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerMDR/GetListSummary?filterByOption=" + filterByOption + "&sortByOption=" + sortByOption
                + "&filterValue=" + filterValue + "&projectId=" + project.Id, authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(MDRDocumentListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMDRDocumentDetails([FromQuery] long id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerMDR/GetMDRDocumentDetails?id={id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(List<MDRDocumentCommentListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMDRDocumentComments([FromQuery] long id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerMDR/GetMDRDocumentComments?id={id}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(List<MDRDocumentCommentListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMDRDocumentStatusHistory(long id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerMDR/GetMDRDocumentStatusHistory?id={id}",
                authorizationToken: accessToken);
            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(MDRDocumentCommentListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCommentDetails([FromQuery]long commentId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerMDR/GetCommentDetails?commentId={commentId}",
                authorizationToken: accessToken);
            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(MDRIssuanceDescription), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetIssuanceDescription([FromQuery] long mdrId)
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerMDR/GetIssuanceDescription?mdrId={mdrId}&projectId={project.Id}",
                authorizationToken: accessToken);
            return new ObjectResult(content);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        public IActionResult DownloadMdrZip(string mdrCode,string folderPath)
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);
            var filePath = Path.Combine($"{_enviroment.ContentRootPath}/wwwroot/mdrdocuments/{project.Id}/MDR-{mdrCode}/{folderPath}");
            FormDocumentFileHelper docHelper = new FormDocumentFileHelper();
            var zipItem = docHelper.ZipFolder(filePath);
            return File(zipItem, "application/zip", $"MDR-{mdrCode}-Archive.zip");
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        public async Task<IActionResult> DownloadMdrCommentFile(long commentId)
        {
            var listService = new ListMDRDocumentService(_context);
            var filePath = await listService.GetCommentFilePath(commentId);
            var contentType = "APPLICATION/octet-stream";
            if(!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("Can't find specific command file.");
            }

            var fileName =Path.GetFileName(filePath);
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType, fileName);
        }

        //
        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        public async Task<IActionResult> CreateMDRDocument(MDRDocumentDto model,
          [FromServices]IActionService<IPlaceMDRDocumentAction> service)
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);
            var listService = new ListMDRDocumentService(_context);
            var roadMapService = new ListWorkPackageService(_context);
            var wbsItem = await roadMapService.GetRoadMapAsycn(model.WorkPackageId);

            if (wbsItem == null)
            {
                service.Status.AddError("workPackage not valid!!!", "Project");
            }

            if (listService.HasDublicatedCode(project.Id,model.Code))
            {
                service.Status.AddError("Inserted Code is Taked by other MDR Document.", "MDR");
            }

            if (!service.Status.HasErrors)
            {
                model.ProjectId = project.Id;
                FormDocumentFileHelper docHelper = new FormDocumentFileHelper();

                model.FolderName = "NI";
                if (model.NativeFiles != null)
                {
                    await docHelper.SaveMDRDocumentsNative(model.Code, project.Id.ToString(), model.NativeFiles, _enviroment, "Native", "NI");
                }
                if (model.PDFFiles != null)
                {
                    await docHelper.SaveMDRDocumentsNative(model.Code, project.Id.ToString(), model.PDFFiles, _enviroment, "PDF", "NI");
                }
                if (model.AttachMentFiles!=null)
                {
                    await docHelper.SaveMDRDocumentsNative(model.Code, project.Id.ToString(), model.AttachMentFiles, _enviroment, "Attachment", "NI");
                }
                service.RunBizAction<MDRDocument>(model);
            }

            SetupTraceInfo();
            if (!service.Status.HasErrors)
            {
                return RedirectToAction("CreateMDRDocument");
            }

            service.Status.CopyErrorsToModelState(ModelState, model);

            var items = await roadMapService.GetRoadMapsAsync();
            ViewBag.ProjectWBSs = items;
            return View(new MDRDocumentListDto());//Used to update the logs
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        public async Task<IActionResult> MDRDocumentDetails(IssuanceDto model
           , [FromServices]IActionService<IUpdateDocumentIssuance> service)
        {
            if (model.NativeFiles == null)
            {
                service.Status.AddError("Document Native File is required.", "MDR");
            }

            if (model.PDFFiles == null)
            {
                service.Status.AddError("Document PDF File is required.", "MDR");
            }

            if(string.IsNullOrWhiteSpace(model.NextStatus))
            {
                service.Status.AddError("Next Status not valid.", "MDR");
            }

            if (!service.Status.HasErrors)
            {
                var projectService = new ListProjectService(_context);
                var user = User.GetCurrentUserDetails();
                var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
                var project = projectService.GetProject(cpid);
                string folderName = $"{model.NextStatus}-{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";

                FormDocumentFileHelper docHelper = new FormDocumentFileHelper();
                await docHelper.SaveMDRDocumentsNative(model.Code, project.Id.ToString(), model.NativeFiles, _enviroment, "Native", folderName);
           
                await docHelper.SaveMDRDocumentsNative(model.Code, project.Id.ToString(), model.PDFFiles, _enviroment, "PDF", folderName);
         
                if (model.AttachMentFiles != null)
                {
                    string attachFilePaths = await docHelper.SaveMDRDocumentsNative(model.Code, project.Id.ToString(), model.AttachMentFiles, _enviroment, "Attachment", folderName);
                
                }
                model.FolderName = folderName;
                model.ProjectId = project.Id;
                service.RunBizAction(model);
                if (!service.Status.HasErrors)
                {
                    return RedirectToAction("MDRDocumentDetails", new { id = model.MdrId });
                }
            }

            service.Status.CopyErrorsToModelState(ModelState, model);
            return View(model.MdrId);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        public async Task<IActionResult> EditMDRDocument(MDRDocumentDto model,
         [FromServices]IActionService<IUpdateMDRDocumentAction> service)
        {
            var projectService = new ListProjectService(_context);
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var project = projectService.GetProject(cpid);
            var listService = new ListMDRDocumentService(_context);

            var roadMapService = new ListWorkPackageService(_context);
            var wbsItem = await roadMapService.GetRoadMapAsycn(model.WorkPackageId);

            if (wbsItem == null)
            {
                service.Status.AddError("workPackage not valid!!!", "Project");
            }
            var oldMdr = await listService.GetMdrDocument(model.Id);
            if (!service.Status.HasErrors)
            {
                if (!string.Equals(model.Code, oldMdr.Code))
                {
                    if (!listService.HasDublicatedCode(project.Id, model.Code))
                    {
                        FormDocumentFileHelper docHelper = new FormDocumentFileHelper();
                        docHelper.renameMDRDocumentsFolder(oldMdr.Code, project.Id.ToString(), model.Code, _enviroment);
                    }
                    else
                    {
                        service.Status.AddError("Inserted Code is Taked by other MDR Document.", "MDR");
                    }
                }
            }

            if(!service.Status.HasErrors)
            {
                service.RunBizAction(model);

                SetupTraceInfo();

                return RedirectToAction("EditMDRDocument", new { id = model.Id });
            }

            service.Status.CopyErrorsToModelState(ModelState, model);
            ViewBag.ProjectWBSs = await roadMapService.GetRoadMapsAsync();
            SetupTraceInfo();       //Used to update the logs
            return View(oldMdr);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPost]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseModelDto<MDRDocumentListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddMDRComment(IFormFile File, [FromForm] string jsonString,
            [FromServices]IActionService<IPlaceMDRCommentAction> service)
        {
            var model = JsonConvert.DeserializeObject<MDRDocumentCommentDto>(jsonString);
            model.File = File;
            var listService = new ListMDRDocumentService(_context);
            var projectService = new ListProjectService(_context);

            var mdrList = await listService.GetMdrDocument(model.MDRDocumentId);

            if (mdrList == null)
            {
                service.Status.AddError("MDR Not Valid!!!", "Project");
            }

            if (!service.Status.HasErrors)
            {
                model.CreateDate = DateTime.Now;
                if (File != null)
                {
                    var user = User.GetCurrentUserDetails();
                    var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
                    var project = projectService.GetProject(cpid);

                    FormDocumentFileHelper docHelper = new FormDocumentFileHelper();
                    string filePath = await docHelper.SaveMDRDocumentComment(mdrList.Code, project.Id.ToString(), File, _enviroment);
                    model.FilePath = filePath;
                }

                var MDRDoc = service.RunBizAction<MDRDocumentComment>(model);
                if(!service.Status.HasErrors)
                {
                    SetupTraceInfo();

                    return new ObjectResult(new SuccessfullyResponseModelDto<MDRDocumentListDto>
                    {
                        Key = 200,
                        Value = "Create MDR Comment success!!"
                    });
                }
            }

            service.Status.CopyErrorsToModelState(ModelState, model);
            return new ObjectResult(new SuccessfullyResponseModelDto<MDRDocumentListDto> { Key = -1, Value = service.Status.GetAllErrors() });
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpPut]
        [Route("poec/[controller]/[action]")]
        [ProducesResponseType(typeof(SuccessfullyResponseDto), (int)HttpStatusCode.OK)]
        public SuccessfullyResponseDto EditMdrComment([FromBody] MDRDocumentCommentDto model
            , [FromServices]IActionService<IUpdateMDRCommentAction> service)
        {
            service.RunBizAction(model);
            if(!service.Status.HasErrors)
            {
                return new SuccessfullyResponseDto { Key = 200, Value = "Update MDR Comment success" };
            }
            service.Status.CopyErrorsToModelState(ModelState, model);
            return new SuccessfullyResponseDto { Key = -1, Value = service.Status.GetAllErrors() };
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        public JsonResult GetFilterSearchContent(MDRDocumentSortFilterPageOptions options)
        {
            var service = new MDRDocumentFilterDropdownService(_context);
            
            var traceIdent = HttpContext.TraceIdentifier; //This makes the logging display work

            return Json(
                new TraceIndentGeneric<IEnumerable<DropdownTuple>>(
                traceIdent,
                service.GetFilterDropDownValues(options.FilterBy)));
        }
    }
}
