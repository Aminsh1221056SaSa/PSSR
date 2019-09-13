using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.Utils.ChartsDto;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using PSSR.ServiceLayer.ProjectSystemServices.Concrete;
using PSSR.ServiceLayer.DesciplineServices.Concrete;
using PSSR.ServiceLayer.WorkPackageSteps.Concrete;

namespace PSSR.API.Controllers
{
    [ApiVersion("1.0")]
    public class ChartReportController : BaseManagerController
    {
        private readonly EfCoreContext _context;
        public ChartReportController(EfCoreContext context)
        {
            _context = context;
        }

        #region workpackage

        #endregion

        #region activity

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(BarChartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllActivityTaskDonePerDayForUser(int personId)
        {
            var projectService = new ListProjectService(_context);
            return new ObjectResult(await projectService.GetAllActivityTaskDonePerDayForUser(personId));
        }

        #endregion

        #region system charts

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(BarChartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskStatusBySystem()
        {
            var systemService = new ListProjectSystemService(_context);
            var viewModel = await systemService.getActivityStatusBySystem();

            return new ObjectResult(viewModel);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(BarChartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskConditionBySystem()
        {
            var systemService = new ListProjectSystemService(_context);
            var viewModel = await systemService.getActivityConditionBySystem();

            return new ObjectResult(viewModel);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PieChartsListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskCounterBySystem()
        {
            var systemService = new ListProjectSystemService(_context);
            var viewModel = await systemService.getActivityCounterBySystem();

            return new ObjectResult(viewModel);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(BarChartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskDoneBySystem()
        {
            var systemService = new ListProjectSystemService(_context);
            var viewModel = await systemService.getActivityTaskDoneBySystem();

            return new ObjectResult(viewModel);
        }

        #endregion

        #region descipline reports

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(BarChartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskStatusPreCommByDesciplines(int workPackageId, int locationId, bool total, Guid projectId)
        {
            var desciplineService = new ListDesciplineService(_context);
            var viewModel = await desciplineService.getActivityStatusByDesciplineForWorkPackage(workPackageId, locationId, total, projectId);

            return new ObjectResult(viewModel);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(BarChartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskConditionByDesciplines()
        {
            var desciplineService = new ListDesciplineService(_context);
            var viewModel = await desciplineService.getActivityConditionByDescipline();

            return new ObjectResult(viewModel);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PieChartsListDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskCounterByDesciplines()
        {
            var desciplineService = new ListDesciplineService(_context);
            var viewModel = await desciplineService.getActivityCounterByDescipline();

            return new ObjectResult(viewModel);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(BarChartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskDoneByDesciplines()
        {
            var desciplineService = new ListDesciplineService(_context);
            var viewModel = await desciplineService.getActivityTaskDoneByDescipline();

            return new ObjectResult(viewModel);
        }

        #endregion


        #region workpackageStepreports

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(BarChartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TaskStatusPreCommByWorkStep(int workPackageId, int locationId, bool total, Guid projectId)
        {
            var listService = new WorkPackageStepService(_context);
            var viewModel = await listService.getActivityStatusByWorkStepForWorkPackage(workPackageId, locationId, total, projectId);

            return new ObjectResult(viewModel);
        }

        #endregion

    }
}