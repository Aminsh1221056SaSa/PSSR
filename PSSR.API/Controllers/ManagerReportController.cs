using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using PSSR.API.Models.Dtos;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ProjectServices;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using PSSR.ServiceLayer.RoadMapServices.Concrete;
using PSSR.ServiceLayer.Utils.ReportsDto;
using PSSR.ServiceLayer.Utils.WorkPackageReportDto;

namespace PSSR.API.Controllers
{
    [ApiVersion("1.0")]
    public class ManagerReportController : BaseManagerController
    {
        private readonly EfCoreContext _context;
        private readonly IMapper _mapper;
        public ManagerReportController(EfCoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ProjectDashboardDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ProjectDashboard(Guid projectId)
        {
            var projectService = new ListProjectService(_context);
            var projectformat = await projectService.GetProjectFormatedDate(projectId);
            var roadMapService = new ListWorkPackageService(_context);

            var viewModel = new ProjectDashboardDto();
            if (projectformat != null)
            {
                viewModel.Project = projectformat;
                viewModel.WorkPackages = await roadMapService.GetRoadMapsAsync();
            }

            return new ObjectResult(viewModel);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ProjectDashboardDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ManagerDashboard(Guid projectId)
        {
            var projectService = new ListProjectService(_context);
            var wbsServices = new ListWBSService(_context,_mapper);
            var projectFormated = await projectService.GetProjectFormatedDate(projectId);
            var roadMapService = new ListWorkPackageService(_context);

            var viewModel = new ManagerDashboardDto();
            if (projectFormated != null)
            {
                viewModel.Project = projectFormated;
                viewModel.WorkPackages = await roadMapService.GetTwoFirstWorkPackagesAsync();
                var targetIds = viewModel.WorkPackages.Select(s => Convert.ToInt64(s.Id)).ToArray();
                viewModel.Locations = (await wbsServices.GetWbsTargetChilderen(projectId,targetIds, Common.WBSType.WorkPackage));
            }

            return new ObjectResult(viewModel);
        }

        #region work Package

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ManagerDashboardWorkPackageReport), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActivityDetailsByWorkPackage(int workPackageId,int groupType,Guid projectId)
        {
            var listService = new ListWorkPackageService(_context);
            var viewModel = await listService.GetActivityDetailsByWorkPackage(projectId, workPackageId, groupType);

            return new ObjectResult(viewModel);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IEnumerable<WFReportList>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDesciplineWorkPackageReport(int workPackageId,Guid projectId)
        {
            var systemService = new ListReportService(_context,_mapper);
            var viewModel = await systemService.GetWorkPackageDesciplineProgress(workPackageId,projectId);

            return new ObjectResult(viewModel);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IEnumerable<WFReportList>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSystemWorkPackageReport(int workPackageId, Guid projectId)
        {
            var systemService = new ListReportService(_context, _mapper);
            var viewModel = await systemService.GetWorkPackageSystemProgress(workPackageId, projectId);
            return new ObjectResult(viewModel);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IEnumerable<WFReportList>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWorkPackageStepProgress(int workPackageId, Guid projectId)
        {
            var systemService = new ListReportService(_context, _mapper);
            var viewModel = await systemService.GetWorkPackageStepProgress(workPackageId, projectId);
            return new ObjectResult(viewModel);
        }

        #endregion

        #region excel reports

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<WBSExcelDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetExportData(bool toActivity, bool calcProgress, Guid projectId)
        {
            var reportService = new ListReportService(_context, _mapper);
            var items = await reportService.GetWBSExportData(projectId, toActivity,calcProgress);
            return new ObjectResult(items);
        }

        #endregion

        #region Task reports

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ClearPunchReportDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDailyTaskFormTypeReport(DateTime fromDate, DateTime toDate, Guid projectid,int workId)
        {
            var reportService = new ListReportService(_context, _mapper);
            return new ObjectResult(await reportService.GetDailyTaskFormTypeReport(fromDate, toDate, projectid, workId));
        }

        #endregion

        #region punch report

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ClearPunchReportDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDailyPunchClearReport(DateTime fromDate, DateTime toDate, Guid projectid)
        {
            var reportService = new ListReportService(_context, _mapper);
            return new ObjectResult(await reportService.GetDailyPunchClearReport(fromDate, toDate, projectid));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PunchCategoryReportDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPunchCategoryReport(DateTime fromDate, DateTime toDate, Guid projectid)
        {
            var reportService = new ListReportService(_context, _mapper);
            return new ObjectResult(await reportService.GetPunchCategoryReport(fromDate, toDate, projectid));
        }
        #endregion

        #region status report

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<StatusReportDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetStatusReport(Guid projectid,int workId)
        {
            var reportService = new ListReportService(_context, _mapper);
            return new ObjectResult(await reportService.GetStatusReport(projectid,workId));
        }

        #endregion
    }
}