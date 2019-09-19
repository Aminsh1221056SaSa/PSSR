using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using JqueryDataTables.ServerSide.AspNetCoreWeb.ActionResults;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Web.Http;
using Newtonsoft.Json;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ActivityServices;
using PSSR.ServiceLayer.ActivityServices.Concrete;
using PSSR.ServiceLayer.ProjectServices;
using PSSR.ServiceLayer.Utils.ReportsDto;
using PSSR.UI.Configuration;
using PSSR.UI.Helpers.CashHelper;
using PSSR.UI.Helpers.ExcelHelper;
using PSSR.UI.Helpers.Http;
using PSSR.UI.Helpers.Security;

namespace PSSR.UI.Controllers
{
    [ApiVersion("1.0")]
    public class ReportController : BaseTraceController
    {
        private readonly EfCoreContext _context;
        private readonly IMasterDataCacheOperations _masterDataCache;
        private readonly IHttpClient _clientService;
        private readonly IOptions<ApplicationSettings> _settings;
        private readonly IExcelReportHelper _excelReportHelper;

        public ReportController(EfCoreContext context,IMasterDataCacheOperations masterDataCache
            , IOptions<ApplicationSettings> settings, IHttpClient clientService, IExcelReportHelper excelReportHelper)
        {
            this._context = context;
            this._masterDataCache = masterDataCache;
            this._clientService = clientService;
            this._settings = settings;
            this._excelReportHelper = excelReportHelper;
        }

        public IActionResult SummaryWorkPackageReport(int wrokId,int locationId, bool isPlan, Guid pid = default(Guid))
        {
            var user = User.GetCurrentUserDetails();
            _masterDataCache.SetUserCurrentProject(user.Name, pid);
            var t = new Tuple<int,int, bool>(wrokId,locationId, isPlan);
            return View(t);
        }

        public IActionResult ClearPunchDesciplineDailyReport()
        {
            return View();
        }

        public IActionResult PunchByCategoryReport()
        {
            return View();
        }

        public IActionResult DailyTaskFormTypeReport()
        {
            return View();
        }

        public IActionResult StatusReport(int workId)
        {
            return View(workId);
        }

        #region wbs

        [Authorize(Policy = "dataEventRecordsManager")]
        public async Task<IActionResult> WbsExportProgress(CancellationToken cancellationToken)
        {
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerReport/GetExportData?toActivity={false}&calcProgress={true}&projectId={cpid}",
                authorizationToken: accessToken);
            var models = JsonConvert.DeserializeObject<List<WBSExcelDto>>(content);

            var stream = await _excelReportHelper.WBSProgressReportGroupe(models);
            stream.Position = 0;
            string excelName = $"WBSGroupList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        public async Task<IActionResult> WbsExportWF(CancellationToken cancellationToken)
        {
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerReport/GetExportData?toActivity={false}&calcProgress={false}&projectId={cpid}",
                authorizationToken: accessToken);
            var models = JsonConvert.DeserializeObject<List<WBSExcelDto>>(content);

            var stream = await _excelReportHelper.WBSWeithFactorReportGroupe(models);
            stream.Position = 0;
            string excelName = $"WBSList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [Authorize(Policy = "dataEventRecordsManager")]
        public async Task<IActionResult> WbsExportWFToActivity(CancellationToken cancellationToken)
        {
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerReport/GetExportData?toActivity={true}&calcProgress={false}&projectId={cpid}",
                authorizationToken: accessToken);
            var models = JsonConvert.DeserializeObject<List<WBSExcelDto>>(content);

            var stream = await _excelReportHelper.WBSWeithFactorReportGroupe(models);
            stream.Position = 0;
            string excelName = $"WBSList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        #endregion

        #region activity

        [Authorize(Policy = "dataEventRecordsManager")]
        public async Task<IActionResult> GetActivityExportExcel()
        {
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);
            var param= await _masterDataCache.GetMasterDataCacheAsync<JqueryDataTablesParameters>(user.Name + "GetActivityDataTable");
            var activityService = new ListActivityService(_context);
            var results = await activityService.GetExcelExportDataTableAsync(param,cpid);
            return new JqueryDataTablesExcelResult<ActivityListExcelDataTableDto>(results.Items, "Activity", $"ActivityList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}");
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(ClearPunchReportDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDailyTaskFormTypeReport([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, [FromQuery]int workId)
        {
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerReport/GetDailyTaskFormTypeReport?projectId={cpid}&fromDate={fromDate}&toDate={toDate}&workId={workId}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }
        #endregion

        #region punch report

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(ClearPunchReportDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDailyPunchClearReport([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerReport/GetDailyPunchClearReport?projectId={cpid}&fromDate={fromDate}&toDate={toDate}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(PunchCategoryReportDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPunchCategoryReport([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerReport/GetPunchCategoryReport?projectId={cpid}&fromDate={fromDate}&toDate={toDate}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }
        #endregion

        #region status report
      
        [HttpGet]
        [Route("APSE/[controller]/[action]")]
        [ProducesResponseType(typeof(PunchCategoryReportDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetStatusReport([FromQuery] int workId)
        {
            var user = User.GetCurrentUserDetails();
            var cpid = _masterDataCache.GetUserCurrentProject(user.Name);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var content = await _clientService.GetStringAsync($"{_settings.Value.OilApiAddress}ManagerReport/GetStatusReport?projectId={cpid}&workId={workId}",
                authorizationToken: accessToken);

            return new ObjectResult(content);
        }

        #endregion
    }
}