
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;

using PSSR.API.Models.Dtos;
using PSSR.Common;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ActivityServices;
using PSSR.ServiceLayer.ActivityServices.Concrete;
using PSSR.ServiceLayer.ProjectServices;

namespace PSSR.API.Controllers
{
    [ApiVersion("1.0")]
    public class ManagerActivityController : BaseManagerController
    {
        private readonly EfCoreContext _context;
        public ManagerActivityController(EfCoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(CustomOptionModelDto<ActivitySortFilterPageOptions, ActivityListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ActivityListSummary(string filterByOption,string sortByOption
            , string filterValue,int pageNum,int pageSize, Guid projectId, string query = "",string prevCheckState = "")
        {
            var listService = new ListActivityService(_context);

            ActivitySortFilterPageOptions options = new ActivitySortFilterPageOptions();

            options.FilterBy = filterByOption.ParseEnum<ServiceLayer.ActivityServices.QueryObjects.ActivityFilterBy>();
            options.OrderByOptions = sortByOption.ParseEnum<ServiceLayer.ActivityServices.QueryObjects.OrderByOptions>();
            options.FilterValue = filterValue;
            options.PageNum = pageNum;
            options.PageSize = pageSize;
            options.PrevCheckState = prevCheckState;
            options.QueryFilter = query;

            var activityList = (await listService.SortFilterPage(options, projectId)).ToList();

            var viewModel = new CustomOptionModelDto<ActivitySortFilterPageOptions, ActivityListDto>(options, activityList);

            return new ObjectResult(viewModel);
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(CustomOptionModelDto<ActivitySortFilterPageOptions, ActivityListDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ActivityListSummaryByWorkDescipline(int workId,int desId,string filterByOption,string sortByOption
           ,string filterValue,int pageNum,int pageSize, Guid projectId, string query = "",string prevCheckState = "")
        {
            var listService = new ListActivityService(_context);

            ActivitySortFilterPageOptions options = new ActivitySortFilterPageOptions();
            if (!string.IsNullOrWhiteSpace(filterByOption))
                options.FilterBy = filterByOption.ParseEnum<ServiceLayer.ActivityServices.QueryObjects.ActivityFilterBy>();
            if (!string.IsNullOrWhiteSpace(sortByOption))
                options.OrderByOptions = sortByOption.ParseEnum<ServiceLayer.ActivityServices.QueryObjects.OrderByOptions>();
            options.FilterValue = filterValue;
            options.PageNum = pageNum;
            options.PageSize = pageSize;
            options.PrevCheckState = prevCheckState;
            options.QueryFilter = query;

            var activityList = (await listService.SortFilterByDesciplineWorkPackagePage(options, workId, desId,projectId)).ToList();

            var viewModel = new CustomOptionModelDto<ActivitySortFilterPageOptions, ActivityListDto>(options, activityList);

            return new ObjectResult(viewModel);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ActivityListDetailsDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActivityDetails(long id)
        {
            var listService =
                  new ListActivityService(_context);
            var model = await listService.GetActivity(id);
            return new ObjectResult(model);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ProjectWBSListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActivityWBSTree(long activityWBsId,Guid projectId)
        {
            var activityService = new ListActivityService(_context);
            var activity = await activityService.GetActivity(activityWBsId);
            return new ObjectResult(await activityService.GetActivityWBSTree(activity, projectId));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ActivityStatusHistoryListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActivityStatusHistory(long activityId)
        {
            var activityService = new ListActivityService(_context);
            var lstHistory = await activityService.GetStatusHistory(activityId);
            return new ObjectResult(lstHistory);
        }
    }
}