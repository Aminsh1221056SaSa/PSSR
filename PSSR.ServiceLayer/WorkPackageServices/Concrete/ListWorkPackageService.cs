using Microsoft.EntityFrameworkCore;
using PSSR.Common;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ActivityServices;
using PSSR.ServiceLayer.Utils;
using PSSR.ServiceLayer.Utils.ProgressHelper;
using PSSR.ServiceLayer.Utils.WorkPackageReportDto;
using PSSR.ServiceLayer.WorkPackageServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.RoadMapServices.Concrete
{
    public class ListWorkPackageService
    {
        private readonly EfCoreContext _context;

        public ListWorkPackageService(EfCoreContext context)
        {
            _context = context;
        }

        public async Task<List<WorkPackageListDto>> GetTwoFirstWorkPackagesAsync()
        {
            return await _context.ProjectRoadMaps.OrderBy(s=>s.Id).Take(2).Select(s => new WorkPackageListDto
            {
                Id = s.Id,
                Title = s.Name,
            }).ToListAsync();
        }

        public async Task<List<WorkPackageListDto>> GetRoadMapsAsync()
        {
            return await _context.ProjectRoadMaps.Select(s => new WorkPackageListDto
            {
                Id=s.Id,
                Title=s.Name,
            }).ToListAsync();
        }

        public async Task<List<LocationListDto>> GetLocationsAsync()
        {
            return await _context.LocationTypes.Select(s => new LocationListDto
            {
                Id = s.Id,
                Title = s.Title
            }).ToListAsync();
        }

        public async Task<WorkPackageListDto> GetRoadMapAsycn(int id)
        {
            return await _context.ProjectRoadMaps.Where(s => s.Id == id).Select(s => new WorkPackageListDto
            {
                Id = s.Id,
                Title = s.Name,
            }).FirstOrDefaultAsync();
        }

        public async Task<LocationListDto> GetLocationAsycn(int id)
        {
            return await _context.LocationTypes.Where(s => s.Id == id).Select(s => new LocationListDto
            {
               Id=s.Id,
               Title=s.Title
            }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<WorkPackageActivityListDto>> GetWorkPackageToActivityCount()
        {
            var items = await _context.ProjectRoadMaps.ToListAsync();

            var allWorkIds = items.Select(s => s.Id).ToArray();

            var allTaskDic = await _context.Activites.Where(s => allWorkIds.Contains(s.WorkPackageId))
                .Select(s => new { s.WorkPackageId, s.Status }).ToListAsync();

            var lstItems = new List<WorkPackageActivityListDto>();

            Parallel.ForEach(items, item =>
            {
                var relatedTask = allTaskDic.Where(s => s.WorkPackageId==item.Id);

                var addItem = new WorkPackageActivityListDto
                {
                    Id = item.Id,
                    Name = item.Name,
                };

                if (relatedTask.Any())
                {
                    addItem.ActivityCount = relatedTask.Count();
                    addItem.CountByStatus = relatedTask.GroupBy(s => s.Status).ToDictionary(s => s.Key, v => v.Count());
                }

                lstItems.Add(addItem);
            });

            return lstItems;
        }

        //reports
        public async Task<ManagerDashboardWorkPackageReport> GetActivityDetailsByWorkPackage(Guid projectId, int workPackageId,int groupType)
        {
            var projectSystems = await _context.ProjectSystems
                .Where(s => s.ProjectId == projectId).ToListAsync();

            var sIds = projectSystems.Select(s => s.Id).ToArray();

            var subSystemIds = await _context.ProjectSubSystems.Where(s => sIds.Contains(s.ProjectSystemId))
                .Select(s => s.Id).ToArrayAsync();

            var allwbsItems = await _context.ProjectWBS.Where(s=>s.ProjectId==projectId)
                .Include(s => s.Parent).Include(s => s.Childeren).AsNoTracking().ToListAsync();

            var planWbsItems = await _context.ProjectWBS.Where(s => s.ProjectId == projectId)
                .Include(s => s.Parent).Include(s => s.Childeren).AsNoTracking().ToListAsync();// HelperExtensions.DeepClone(allwbsItems);

            var allActivity = await _context.Activites.Where(s =>subSystemIds.Contains(s.SubsytemId) && s.WorkPackageId==workPackageId)
                .Include(s=>s.Punchs).Include(s=>s.StatusHistory).Include(s=>s.SubSystem).ToListAsync();

            var desciplens = await _context.Desciplines.ToListAsync();

            var pHelper = new ProgressHelper();
            //calculate Done Activity progress
            var calculateActivity = new List<ActivityListDetailsDto>();
            allActivity.Where(s => s.Status == ActivityStatus.Done).ForEach(s=>
            {
                calculateActivity.Add(new ActivityListDetailsDto
                {
                    WorkPackageId = s.WorkPackageId,
                    LocationId = s.LocationId,
                    DesciplineId = s.DesciplineId,
                    SubsytemId = s.SubsytemId,
                    WeightFactor = s.WeightFactor,
                    Progress = s.Progress,
                    Id = s.Id,
                    TagNumber = s.TagNumber,
                    SystemdId = s.SubSystem.ProjectSystemId,
                    RPlanEndDate=s.PlanEndDate,
                    RPlanStartDate=s.PlanStartDate
                });
            });

            var activityPlane = calculateActivity.Where(s => s.RPlanEndDate.HasValue && s.RPlanEndDate < DateTime.Now.Date).ToList();

            var wbsItems = pHelper.CalculateProgress(allwbsItems, calculateActivity, false);
            var planewbsItems = pHelper.CalculateProgress(planWbsItems, activityPlane, false);
            var viewModelDto = await this.reportActualWorkPackage(allActivity, wbsItems,planewbsItems, pHelper, workPackageId,projectId);
            
            if (groupType==1)
            {
                viewModelDto.GroupReport = this.reportActivityDescplineGroupedByWorkPackage(allActivity, desciplens,workPackageId);
            }
            else if(groupType==2)
            {
                viewModelDto.GroupReport = this.reportActivitySystemGroupedByWorkPackage(allActivity, projectSystems, workPackageId);
            }

            return viewModelDto;
        }

        //private methods
        private async Task<ManagerDashboardWorkPackageReport> reportActualWorkPackage(IEnumerable<Activity> allActivity,
            IEnumerable<ProjectWBS> wbsItems, IEnumerable<ProjectWBS> planwbsItems, ProgressHelper pHelper, int workPackageId,Guid projectId)
        {
            var viewmodelDto = new ManagerDashboardWorkPackageReport();

            var lstItemsActual = new List<ManagerWorkPackageDto>();
            var lstItemsPlan = new List<ManagerWorkPackageDto>();

            var filterWbs = wbsItems.Where(s => s.ParentId.HasValue && s.Parent.TargetId == workPackageId &&
             s.Parent.Type == WBSType.WorkPackage);

            foreach(var s in filterWbs)
            {
                pHelper.progressRecovery(s);
                lstItemsActual.Add(new ManagerWorkPackageDto
                {
                    Title = $"% Actual {s.Name}",
                    Value = s.Progress,
                    IsBolded = true,
                    Link = $"/Report/summaryworkpackagereport?wrokId={workPackageId}&locationId={s.TargetId}&isPlan=false"
                });
            }

            if(planwbsItems.Any())
            {
                var filterWbsplan = planwbsItems.Where(s => s.ParentId.HasValue && s.Parent.TargetId == workPackageId &&
                s.Parent.Type == WBSType.WorkPackage);

                foreach (var s in filterWbsplan)
                {
                    pHelper.progressRecovery(s);
                    lstItemsPlan.Add(new ManagerWorkPackageDto
                    {
                        Title = $"% Plan {s.Name}",
                        Value = s.Progress,
                        IsBolded = true,
                        Link = $"/Report/summaryworkpackagereport?wrokId={workPackageId}&locationId={s.TargetId}&isPlan=true"
                    });
                }
            }

            var notDoneActivity = allActivity.Where(s => s.Status != ActivityStatus.Done);

            int remainTaskCount = notDoneActivity.Count();
            lstItemsActual.Add(new ManagerWorkPackageDto
            {
                Title = "Remain Task",
                Value = remainTaskCount,
                IsBolded = false,
                IsTitle = true,
                Link = $"/ProjectManagment/Activity/ActivityList?WorkPackageId={workPackageId}&FilterBy=1&FilterValue=NotStarted"
            });

            //int holdByMaterial = notDoneActivity.Where(s => s.Condition == ActivityCondition.Hold &&
            //s.StatusHistory.Any(o => o.HoldBy == ActivityHolBy.HoldMaterial)).Count();

            int holdByMaterial = notDoneActivity.Where(s => s.Condition == ActivityCondition.Hold).Count();

            lstItemsActual.Add(new ManagerWorkPackageDto
            {
                Title = "Task-Hold By Material",
                Value = holdByMaterial,
                IsBolded = false,
                Link = $"/ProjectManagment/Activity/ActivityList?WorkPackageId={workPackageId}&FilterBy=2&FilterValue={ActivityCondition.Hold}"
            });

            int holdBySequnce = notDoneActivity.Where(s => s.Condition == ActivityCondition.Hold &&
            s.StatusHistory.Any(o => o.HoldBy == ActivityHolBy.HoldSequence)).Count();

            lstItemsActual.Add(new ManagerWorkPackageDto
            {
                Title = "Task-Hold By Sequence",
                Value = holdBySequnce,
                IsBolded = false,
                Link = $"/ProjectManagment/Activity/ActivityList?WorkPackageId={workPackageId}&FilterBy=2&FilterValue={ActivityCondition.Hold}"
            });

            int holdByOther = notDoneActivity.Where(s => s.Condition == ActivityCondition.Hold &&
            s.StatusHistory.Any(o => o.HoldBy == ActivityHolBy.HoldOther)).Count();

            lstItemsActual.Add(new ManagerWorkPackageDto
            {
                Title = "Task-Hold By Other",
                Value = holdByOther,
                IsBolded = false,
                Link = $"/ProjectManagment/Activity/ActivityList?WorkPackageId={workPackageId}&FilterBy=2&FilterValue={ActivityCondition.Hold}"
            });

            //plan report
            var AllFrontActivity = allActivity.Where(s => s.Status != ActivityStatus.Done && s.Condition == ActivityCondition.Front);

            int remainTaskCountPlan = AllFrontActivity.Count();
            lstItemsPlan.Add(new ManagerWorkPackageDto
            {
                Title = "Task-Front",
                Value = remainTaskCount,
                IsBolded = false,
                IsTitle = true,
                Link = $"/ProjectManagment/Activity/ActivityList?WorkPackageId={workPackageId}&FilterBy=2&FilterValue={ActivityCondition.Front}"
            });

            //punch reported
            var reportedPunch = await _context.PunchTypes.Where(s=>s.ProjectId==projectId).SelectMany(o => o.WorkPackages)
                .Where(o => o.WorkPackageId == workPackageId && o.Precentage > 20)
                .OrderByDescending(s => s.Precentage).Include(o => o.PunchType).ToListAsync();

            foreach(var punchr in reportedPunch)
            {
                var currentPunches = allActivity.Select(s => s.Punchs.Where(o => o.PunchTypeId == punchr.PunchTypeId
                && !o.CheckDate.HasValue));
                int remainPunchCount = currentPunches.Sum(s => s.Count());

                lstItemsActual.Add(new ManagerWorkPackageDto
                {
                    Title = $"Remain Punch {punchr.PunchType.Name}",
                    Value = remainPunchCount,
                    IsBolded = false,
                    IsTitle = true,
                    Link = $"/ProjectManagment/ActivityPunch/PunchList?&FilterBy=1&FilterValue={punchr.PunchTypeId}"
                });

                var currentpunchHoldByMaterial = currentPunches.SelectMany(s => s)
                    .Where(s => s.Activity.Condition == ActivityCondition.Hold &&
                s.Activity.StatusHistory.Any(o => o.HoldBy == ActivityHolBy.HoldMaterial)).Count();

                lstItemsActual.Add(new ManagerWorkPackageDto
                {
                    Title = $"Punch {punchr.PunchType.Name} Hold By Material",
                    Value = currentpunchHoldByMaterial,
                    IsBolded = false,
                    Link = "/ProjectManagment/ActivityPunch/PunchList"
                });

                var currentpunchHoldBySqunces = currentPunches.SelectMany(s => s)
                    .Where(s => s.Activity.Condition == ActivityCondition.Hold &&
                    s.Activity.StatusHistory.Any(o => o.HoldBy == ActivityHolBy.HoldSequence)).Count();

                lstItemsActual.Add(new ManagerWorkPackageDto
                {
                    Title = $"Punch {punchr.PunchType.Name} Hold By Sequence",
                    Value = currentpunchHoldBySqunces,
                    IsBolded = false,
                    Link = "/ProjectManagment/ActivityPunch/PunchList"
                });

                var currentpunchHoldByOther = currentPunches.SelectMany(s => s)
                    .Where(s => s.Activity.Condition == ActivityCondition.Hold &&
                    s.Activity.StatusHistory.Any(o => o.HoldBy == ActivityHolBy.HoldOther)).Count();

                lstItemsActual.Add(new ManagerWorkPackageDto
                {
                    Title = $"Punch {punchr.PunchType.Name} Hold By Other",
                    Value = currentpunchHoldByOther,
                    IsBolded = false,
                    Link = "/ProjectManagment/ActivityPunch/PunchList"
                });

                //plan report
                int currentpunchFront = currentPunches.SelectMany(s => s)
                    .Where(s => s.Activity.Condition == ActivityCondition.Front).Count();

                lstItemsPlan.Add(new ManagerWorkPackageDto
                {
                    Title = $"Punch {punchr.PunchType.Name} Front",
                    Value = currentpunchFront,
                    IsBolded = false,
                    IsTitle = true,
                    Link = "/ProjectManagment/ActivityPunch/PunchList"
                });
            }

            lstItemsActual.Add(new ManagerWorkPackageDto
            {
                Title = "Construction Information",
                Value = 0,
                IsBolded = false,
                Link = "/ProjectManagment/ActivityPunch/PunchList"
            });

            viewmodelDto.ActualReport = lstItemsActual;
            viewmodelDto.PlaneReport = lstItemsPlan;

            return viewmodelDto;
        }

        //private BarChartDto reportActivityStatusDoneByDesciplineForWorkPackage(IEnumerable<Activity> allActivity,IEnumerable<Descipline> desciplens)
        //{
        //    var availableDes = allActivity.Where(s => s.DesciplineId.HasValue)
        //        .Select(s => s.DesciplineId.Value).ToArray();

        //    var viewModel = new BarChartDto();
        //    viewModel.Desciplines = desciplens.Where(s => availableDes.Contains(s.Id))
        //        .OrderBy(s => s.Id).Select(s => s.Name).ToList();

        //    BarChartDetails<string, object> aSeriesTotal = new BarChartDetails<string, object>();
        //    aSeriesTotal["name"] = "Total";
        //    var lstTotal = allActivity.GroupBy(s => s.DesciplineId).OrderBy(s => s.Key)
        //           .Select(s => s.Count()).ToList();

        //    aSeriesTotal["color"] = $"#{Common.ActivityStatusColor.DE1515}";

        //    BarChartDetails<string, object> aSeries = new BarChartDetails<string, object>();
        //    aSeries["name"] = ActivityStatus.Done.ToString();
        //    var lstDate = allActivity
        //          .GroupBy(s => s.DesciplineId).OrderBy(s => s.Key)
        //         .Select(s => s.Where(o => o.Status == ActivityStatus.Done).Count()).ToList();

        //    aSeries["color"] = $"#{Common.ActivityStatusColor.A3db08}";

        //    aSeriesTotal["data"] = lstTotal;
        //    viewModel.Values.Add(aSeriesTotal);
        //    aSeries["data"] = lstDate;
        //    viewModel.Values.Add(aSeries);

        //    return viewModel;
        //}

        //public async Task<BarChartDto> reportActivityStatusDoneByWorkStepForWorkPackage(IEnumerable<Activity> allActivity)
        //{
        //    var workStep = await _context.WorkPackageStep.ToListAsync();

        //    allActivity = allActivity.Where(s => s.WorkPackageStepId.HasValue).ToList();

        //    var availableSteps = allActivity.Select(s => s.WorkPackageStepId.Value).ToArray();
        //    var gstatus = allActivity.Where(s => s.Status == ActivityStatus.Done).ToList();

        //    var viewModel = new BarChartDto();
        //    viewModel.Desciplines = workStep.Where(s => availableSteps.Contains(s.Id))
        //        .OrderBy(s => s.Id).Select(s => s.Title).ToList();

        //    BarChartDetails<string, object> aSeriesTotal = new BarChartDetails<string, object>();
        //    aSeriesTotal["name"] = "Total";
        //    var lstTotal = allActivity.GroupBy(s => s.WorkPackageStepId).OrderBy(s => s.Key)
        //            .Select(s => s.Count()).ToList();

        //    aSeriesTotal["color"] = $"#{Common.ActivityStatusColor.DE1515}";

        //    BarChartDetails<string, object> aSeries = new BarChartDetails<string, object>();
        //    aSeries["name"] = ActivityStatus.Done.ToString();
        //    var lstDate = allActivity
        //         .GroupBy(s => s.WorkPackageStepId).OrderBy(s => s.Key)
        //        .Select(s => s.Where(o => o.Status == ActivityStatus.Done).Count()).ToList();

        //    aSeries["color"] = $"#{Common.ActivityStatusColor.A3db08}";

        //    aSeriesTotal["data"] = lstTotal;
        //    viewModel.Values.Add(aSeriesTotal);
        //    aSeries["data"] = lstDate;
        //    viewModel.Values.Add(aSeries);

        //    return viewModel;
        //}

        //private BarChartDto reportActivityStatusByDesciplineForWorkPackage(IEnumerable<Activity> allActivity,IEnumerable<Descipline> desciplens)
        //{
        //    var availableDes = allActivity.Where(s => s.DesciplineId.HasValue)
        //        .Select(s => s.DesciplineId.Value).ToArray();

        //    var gstatus = allActivity.GroupBy(s => s.Status);

        //    var viewModel = new BarChartDto();
        //    viewModel.Desciplines = desciplens.Where(s => availableDes.Contains(s.Id))
        //        .OrderBy(s => s.Id).Select(s => s.Name).ToList();

        //    foreach (var gbyd in gstatus)
        //    {
        //        BarChartDetails<string, object> aSeries = new BarChartDetails<string, object>();
        //        aSeries["name"] = gbyd.Key.ToString();
        //        var lstDate = new List<int>();

        //        if (gbyd.Key == Common.ActivityStatus.Done)
        //        {
        //            aSeries["color"] = $"#{Common.ActivityStatusColor.A3db08}";
        //        }
        //        else if (gbyd.Key == Common.ActivityStatus.NotStarted)
        //        {
        //            aSeries["color"] = $"#{Common.ActivityStatusColor.FF530D}";
        //        }
        //        else if (gbyd.Key == Common.ActivityStatus.Ongoing)
        //        {
        //            aSeries["color"] = $"#{Common.ActivityStatusColor.E8EC26}";
        //        }
        //        else if (gbyd.Key == Common.ActivityStatus.Reject)
        //        {
        //            aSeries["color"] = $"#{Common.ActivityStatusColor.DE1515}";
        //        }

        //        var gByStatus = gbyd.OrderBy(s => s.DesciplineId).GroupBy(s => s.DesciplineId.Value);
        //        lstDate.Add(availableDes.Count());

        //        foreach (var item in gByStatus)
        //        {
        //            lstDate.Add(item.Count());
        //        }
        //        aSeries["data"] = lstDate;
        //        viewModel.Values.Add(aSeries);
        //    }
        //    return viewModel;
        //}

        //public async Task<BarChartDto> reportActivityStatusByWorkStepForWorkPackage(IEnumerable<Activity> allActivity)
        //{
        //    var workStep = await _context.WorkPackageStep.ToListAsync();

        //    allActivity = allActivity.Where(s => s.WorkPackageStepId.HasValue ).ToList();

        //    var availableSteps = allActivity.Select(s => s.WorkPackageStepId.Value).ToArray();
        //    var gstatus = allActivity.GroupBy(s => s.Status);

        //    var viewModel = new BarChartDto();
        //    viewModel.Desciplines = workStep.Where(s => availableSteps.Contains(s.Id))
        //        .OrderBy(s => s.Id).Select(s => s.Title).ToList();

        //    foreach (var gbyd in gstatus)
        //    {
        //        BarChartDetails<string, object> aSeries = new BarChartDetails<string, object>();

        //        aSeries["name"] = gbyd.Key.ToString();
        //        if (gbyd.Key == Common.ActivityStatus.Done)
        //        {
        //            aSeries["color"] = $"#{Common.ActivityStatusColor.A3db08}";
        //        }
        //        else if (gbyd.Key == Common.ActivityStatus.NotStarted)
        //        {
        //            aSeries["color"] = $"#{Common.ActivityStatusColor.FF530D}";
        //        }
        //        else if (gbyd.Key == Common.ActivityStatus.Ongoing)
        //        {
        //            aSeries["color"] = $"#{Common.ActivityStatusColor.E8EC26}";
        //        }
        //        else if (gbyd.Key == Common.ActivityStatus.Reject)
        //        {
        //            aSeries["color"] = $"#{Common.ActivityStatusColor.DE1515}";
        //        }

        //        var lstDate = new List<int>();

        //        var gByStatus = gbyd.OrderBy(s => s.WorkPackageStepId).GroupBy(s => s.WorkPackageStepId.Value);

        //        foreach (var item in gByStatus)
        //        {
        //            lstDate.Add(item.Count());
        //        }
        //        aSeries["data"] = lstDate;

        //        viewModel.Values.Add(aSeries);
        //    }
        //    return viewModel;
        //}

        private IEnumerable<ManagerDashboardGroupDto> reportActivityDescplineGroupedByWorkPackage(IEnumerable<Activity> actualActivity,
            IEnumerable<Descipline> availableDescipline,int workId)
        {
            var allActivity = actualActivity.GroupBy(s => new { s.DesciplineId }).ToList();
            var desciplens = availableDescipline.OrderBy(s => s.Id).ToDictionary(x => x.Id, s => s.Name);

            var lstItems = new List<ManagerDashboardGroupDto>();
            Parallel.ForEach(allActivity, acg =>
            {
                var desc = desciplens[acg.Key.DesciplineId];
                var item = new ManagerDashboardGroupDto
                { Title = desc,Link= $"/Report/StatusReport?workId={workId}" };
                item.Total = acg.Count();
                item.Done = acg.Count(s => s.Status == Common.ActivityStatus.Done);
                lstItems.Add(item);
            });
            return lstItems;
        }

        private IEnumerable<ManagerDashboardGroupDto> reportActivitySystemGroupedByWorkPackage(IEnumerable<Activity> actualActivity,
            IEnumerable<ProjectSystem> allSystems,int workId)
        {
            var allActivity = actualActivity.GroupBy(s => new { s.SubSystem.ProjectSystemId });

            var systems = allSystems.OrderBy(s => s.Id).ToDictionary(x => x.Id, s => s.Code);

            var lstItems = new List<ManagerDashboardGroupDto>();
            Parallel.ForEach(allActivity, acg =>
            {
                var desc = systems[acg.Key.ProjectSystemId];
                var item = new ManagerDashboardGroupDto { Title = desc,Link= $"/Report/StatusReport?workId={workId}" };
                item.Total = acg.Count();
                item.Done = acg.Count(s => s.Status == Common.ActivityStatus.Done);
                lstItems.Add(item);
            });
            return lstItems;
        }

    }
}
