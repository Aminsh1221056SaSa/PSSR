
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.API.Models.Dtos;
using PSSR.Common;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ActivityServices;
using PSSR.ServiceLayer.PunchCategoryServices;
using PSSR.ServiceLayer.PunchCategoryServices.Concrete;
using PSSR.ServiceLayer.PunchServices;
using PSSR.ServiceLayer.PunchServices.Concrete;
using PSSR.ServiceLayer.PunchTypeServices;
using PSSR.ServiceLayer.PunchTypeServices.Concrete;

namespace PSSR.API.Controllers
{
    [Authorize(Policy = "dataEventRecordsManager")]
    [ApiVersion("1.0")]
    public class ManagerPunchController : BaseManagerController
    {
        private readonly EfCoreContext _context;
        public ManagerPunchController(EfCoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PunchTypeListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllPunchTypes(Guid projectId)
        {
            var listService =new ListPunchTypeService(_context);
            var desciplineList = await listService.GetPunchTypes(projectId);
            return new ObjectResult(desciplineList);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PunchTypeListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPunchTypes([FromQuery] int id)
        {
            var listService = new ListPunchTypeService(_context);
            var desciplineList = await listService.GetPunchType(id);
            return new ObjectResult(desciplineList);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PunchCategoryListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPunchCategory([FromQuery] int id)
        {
            var listService = new ListPunchCategoryService(_context);
            var desciplineList = await listService.GetPunchCategory(id);
            return new ObjectResult(desciplineList);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IEnumerable<PunchCategoryListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPunchCategoryes(Guid projectId)
        {
            var listService = new ListPunchCategoryService(_context);
            var desciplineList = await listService.GetProjectPuncheCategories(projectId);
            return new ObjectResult(desciplineList);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(CustomOptionModelDto<PunchSortFilterPageOptions, PunchListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PunchListSummary(string filterByOption,string sortByOption
            ,string filterValue,int pageNum,int pageSize,Guid projectId,string query = "",string prevCheckState = "")
        {
            var listService = new ListPunchService(_context);

            PunchSortFilterPageOptions options = new PunchSortFilterPageOptions();
            options.FilterBy = filterByOption.ParseEnum<ServiceLayer.PunchServices.QueryObjects.PunchFilterBy>();
            options.OrderByOptions = sortByOption.ParseEnum<ServiceLayer.PunchServices.QueryObjects.OrderByOptions>();
            options.FilterValue = filterValue;
            options.PageNum = pageNum;
            options.PageSize = pageSize;
            options.PrevCheckState = prevCheckState;
            options.QueryFilter = query;

            var punchList = (await listService.SortFilterPage(options, projectId)).ToList();

            var viewModel = new CustomOptionModelDto<PunchSortFilterPageOptions, PunchListDto>(options, punchList);

            return new ObjectResult(viewModel);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PunchEditableListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActivityPunchs(long activityId)
        {
            var activityService = new ListPunchService(_context);
            var lstActivity = await activityService.GetActivityPunchs(activityId);
            return new ObjectResult(lstActivity);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PunchEditableListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPunchGoDetails(long punchId)
        {
            var activityService = new ListPunchService(_context);
            var punch = await activityService.GetPunchGoDetails(punchId);
            return new ObjectResult(punch);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PunchListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPunchDetails(long punchId)
        {
            var activityService = new ListPunchService(_context);
            var punch = await activityService.GetPunchDetails(punchId);
            return new ObjectResult(punch);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ActivityStatusHistoryListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPunchDocuments(long punchId)
        {
            var punchService = new ListPunchService(_context);
            var lstHistory = await punchService.GetDocuments(punchId);
            return new ObjectResult(lstHistory);
        }
    }
}