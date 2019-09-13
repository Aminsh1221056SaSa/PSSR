using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.Common;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.MDRDocumentServices;
using PSSR.ServiceLayer.MDRDocumentServices.Concrete;
using PSSR.ServiceLayer.ProjectServices.Concrete;

namespace PSSR.API.Controllers
{
    [ApiVersion("1.0")]
    public class ManagerMDRController : BaseManagerController
    {
        private readonly EfCoreContext _context;
        private readonly IMapper _mapper;
        public ManagerMDRController(EfCoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(MDRDocumentListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetListSummary(string filterByOption,string sortByOption
         ,string filterValue,int pageNum,Guid projectId,string query = "")
        {
            var listService =new ListMDRDocumentService(_context);

            MDRDocumentSortFilterPageOptions options = new MDRDocumentSortFilterPageOptions();
            options.FilterBy = filterByOption.ParseEnum<ServiceLayer.MDRDocumentServices.QueryObjects.MDRDocumentFilterBy>();
            options.OrderByOptions = sortByOption.ParseEnum<ServiceLayer.MDRDocumentServices.QueryObjects.OrderByOptions>();
            options.FilterValue = filterValue;
            options.PageNum = pageNum;

            var mdrList = (await listService.SortFilterPage(options,projectId)).ToList();
            return new ObjectResult(mdrList);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(MDRDocumentListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMDRDocumentDetails(long id)
        {
            var listService =new ListMDRDocumentService(_context, _mapper);
            var mdrDocDetails = await listService.GetMDRDocumentDetails(id);
            return new ObjectResult(mdrDocDetails);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<MDRDocumentCommentListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMDRDocumentComments(long id)
        {
            var listService =new ListMDRDocumentService(_context, _mapper);
            var mdrDocComments = await listService.GetMDRDocumentComments(id);
            return new ObjectResult(mdrDocComments);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<MDRDocumentStatusListModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMDRDocumentStatusHistory(long id)
        {
            var listService = new ListMDRDocumentService(_context, _mapper);
            var mdrDocComments = await listService.GetMDRDocumentStatusHistory(id);
            return new ObjectResult(mdrDocComments);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(MDRDocumentCommentListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCommentDetails(long commentId)
        {
            var listService = new ListMDRDocumentService(_context, _mapper);
            var mdrDocComments = await listService.GetCommentDetails(commentId);
            return new ObjectResult(mdrDocComments);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(MDRIssuanceDescription), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetIssuanceDescription(long mdrId,Guid projectId)
        {
            var listService = new ListMDRDocumentService(_context, _mapper);
            var mdrDocComments = await listService.GetIssuanceDescription(mdrId,projectId);
            return new ObjectResult(mdrDocComments);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<MDRSummaryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectMDRSummary(Guid projectId)
        {
            var listService = new ListMDRDocumentService(_context, _mapper);
            var mdrDocComments = await listService.GetMDRDashbordSummary(projectId);
            return new ObjectResult(mdrDocComments);
        }
    }
}