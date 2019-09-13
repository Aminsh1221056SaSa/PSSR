using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using Newtonsoft.Json;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ProjectServices;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PSSR.API.Controllers
{
    [ApiVersion("1.0")]
    public class ManagerWbsController:BaseManagerController
    {
        private readonly EfCoreContext _context;
        private readonly IMapper _mapper;
        public ManagerWbsController(EfCoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ProjectWBSListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectWBSTree(Guid projectId)
        {
            var wbsService = new ListWBSService(_context, _mapper);
            var items = await wbsService.GetProjectWBSTree(projectId);

            string rItems = JsonConvert.SerializeObject(items, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return new ObjectResult(rItems);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(ProjectWBSListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectWBSTreeToProgress(bool toProgress,Guid projectId)
        {
            var wbsService = new ListWBSService(_context, _mapper);
            var items = await wbsService.GetWBSProgress(projectId, toProgress);

            string rItems = JsonConvert.SerializeObject(items, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return new ObjectResult(rItems);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ProjectWBSListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectWBSActivityTree(long parentId,Guid projectId)
        {
            var wbsService = new ListWBSService(_context, _mapper);
            var items = await wbsService.GetProjectWBSActivityTree(projectId, parentId);

            return new ObjectResult(items);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ProjectWBSListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectWBS(long wbsId)
        {
            var wbsService = new ListWBSService(_context, _mapper);
            var item = await wbsService.GetProjectWBS(wbsId);

            return new ObjectResult(item);
        }
    }
}
