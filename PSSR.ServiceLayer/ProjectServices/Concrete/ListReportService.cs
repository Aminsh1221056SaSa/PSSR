using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PSSR.Common;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ActivityServices;
using PSSR.ServiceLayer.Utils.ProgressHelper;
using PSSR.ServiceLayer.Utils.ReportsDto;
using PSSR.ServiceLayer.Utils.WorkPackageReportDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.ProjectServices.Concrete
{
    public class ListReportService
    {
        private readonly EfCoreContext _context;
        private readonly IMapper _mapper;

        public ListReportService(EfCoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //summary progress report

        public async Task<IEnumerable<WFReportList>> GetWorkPackageDesciplineProgress(int wId, Guid projectId)
        {
            var wbsItems = await _context.ProjectWBS.Where(s => s.ProjectId == projectId)
                .Include(s => s.Childeren).ToListAsync();

            var desciplens = await _context.Desciplines.OrderBy(s => s.Id)
                .ToDictionaryAsync(x => x.Id, s => s.Name);

            var wokWbs = wbsItems.FirstOrDefault(s => s.TargetId == wId && s.Type == WBSType.WorkPackage);
            if (wokWbs == null) return null;
            var pHelper = new ProgressHelper();
            var wChilds = new List<ProjectWBS>();
            pHelper.geChilds(wokWbs, wChilds);
            var locations = wChilds.Where(s => s.Type == WBSType.Location).ToArray();

            var subsystemIds = await _context.ProjectSubSystems
                .Where(s => s.ProjectSystem.ProjectId == projectId).Select(s => s.Id).ToArrayAsync();

            var allActivity = await _context.Activites.Where(s => subsystemIds.Contains(s.SubsytemId) && s.WorkPackageId == wId)
                .Select(s => new ActivityListDetailsDto
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
                    ValueUnitId = s.ValueUnitId,
                    ValueUnitNum = s.ValueUnitNum,
                    Status = s.Status,
                    WorkPackageStepId = s.WorkPackageStepId
                }).ToListAsync();

            var viewModel = new List<WFReportList>();

            pHelper.CalculateActivityWFRelatedToWBSLastParent(wbsItems, allActivity,WBSType.Location);

            foreach (var loc in locations)
            {
                var item = new WFReportList();

                item.LocationId = loc.TargetId;
                foreach (var des in desciplens)
                {
                    var lstItems = new WFReport();

                    lstItems.Name = des.Value;
                    lstItems.Id = des.Key;

                    var desActivity = allActivity.Where(s => s.LocationId == loc.TargetId &&
                       s.DesciplineId == des.Key);

                    if (desActivity.Any())
                    {
                        float calwf = desActivity.Sum(s => s.WeightFactor);
                        var doneActivity = desActivity.Where(s => s.Status == ActivityStatus.Done);
                        lstItems.Wf =desActivity.Sum(s=>s.WeightFactor)*100;
                        lstItems.Progress = doneActivity.Sum(s=>(s.Progress/100)*s.WeightFactor)*100;
                        lstItems.WfCalculate = true;
                        lstItems.PCT = doneActivity.Sum(s => s.Progress * s.WeightFactor) / calwf;
                    }
                    item.Items.Add(lstItems);
                }
                viewModel.Add(item);
            }

            var resultQuery = viewModel.SelectMany(s => s.Items);

            var groupedItems = resultQuery.GroupBy(s => s.Id);
            var itemLast = new WFReportList();

            foreach (var gr in groupedItems)
            {
                var lstItems = new WFReport();
                lstItems.Name = gr.First().Name;
                lstItems.Wf = gr.Sum(s => s.Wf) / locations.Count();
                lstItems.Progress = gr.Sum(s => s.Progress) / locations.Count();
                lstItems.PCT = gr.Sum(s => s.PCT) / locations.Count();

                itemLast.Items.Add(lstItems);
                itemLast.LocationId = 0;
            }

            viewModel.Add(itemLast);
            return viewModel;
        }

        public async Task<IEnumerable<WFReportList>> GetWorkPackageSystemProgress(int wId, Guid projectId)
        {
            var systems = await _context.ProjectSystems.Where(s => s.ProjectId == projectId).OrderBy(s => s.Id)
                 .ToDictionaryAsync(x => x.Id, s => s.Code);

            var wbsItems = await _context.ProjectWBS.Where(s => s.ProjectId == projectId).Include(s => s.Childeren).ToListAsync();

            var wokWbs = wbsItems.FirstOrDefault(s => s.TargetId == wId && s.Type == WBSType.WorkPackage);
            if (wokWbs == null) return null;
            var pHelper = new ProgressHelper();
            var wChilds = new List<ProjectWBS>();
            pHelper.geChilds(wokWbs, wChilds);
            var locations = wChilds.Where(s => s.Type == WBSType.Location).ToArray();

            var sIds = systems.Select(s => s.Key);

            var subsystemIds = await _context.ProjectSubSystems
                 .Where(s => sIds.Contains(s.ProjectSystemId)).Select(s => s.Id).ToArrayAsync();

            var allActivity = await _context.Activites.Where(s => subsystemIds.Contains(s.SubsytemId) && s.WorkPackageId == wId)
                .Select(s => new ActivityListDetailsDto
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
                    ValueUnitId = s.ValueUnitId,
                    ValueUnitNum = s.ValueUnitNum,
                    Status = s.Status,
                    WorkPackageStepId = s.WorkPackageStepId
                }).ToListAsync();

            var viewModel = new List<WFReportList>();

            pHelper.CalculateActivityWFRelatedToWBSLastParent(wbsItems, allActivity,WBSType.Location);

            foreach (var loc in locations)
            {
                var item = new WFReportList();

                item.LocationId = loc.TargetId;
                foreach (var sys in systems)
                {
                    var lstItems = new WFReport();

                    lstItems.Name = sys.Value;
                    lstItems.Id = sys.Key;

                    var sysActivity = allActivity.Where(s => s.LocationId == loc.TargetId &&
                        s.SystemdId == sys.Key);

                    if (sysActivity.Any())
                    {
                        float calwf = sysActivity.Sum(s => s.WeightFactor);
                        var doneActivity = sysActivity.Where(s => s.Status == ActivityStatus.Done);
                        lstItems.Wf = sysActivity.Sum(s => s.WeightFactor) * 100;
                        lstItems.Progress = doneActivity.Sum(s => (s.Progress / 100) * s.WeightFactor) * 100;
                        lstItems.WfCalculate = true;
                        lstItems.PCT = doneActivity.Sum(s => s.Progress * s.WeightFactor) / calwf;
                    }
                    item.Items.Add(lstItems);
                }

                viewModel.Add(item);
            }

            var resultQuery = viewModel.SelectMany(s => s.Items);

            var groupedItems = resultQuery.GroupBy(s => s.Id);
            var itemLast = new WFReportList();

            foreach (var gr in groupedItems)
            {
                var lstItems = new WFReport();
                lstItems.Name = gr.First().Name;
                lstItems.Wf = gr.Sum(s => s.Wf) / locations.Count();
                lstItems.Progress = gr.Sum(s => s.Progress) / locations.Count();
                lstItems.PCT = gr.Sum(s => s.PCT) / locations.Count();

                itemLast.Items.Add(lstItems);
                itemLast.LocationId = 0;
            }

            viewModel.Add(itemLast);
            return viewModel;
        }

        public async Task<IEnumerable<WFReportList>> GetWorkPackageStepProgress(int wId, Guid projectId)
        {
            var wbsItems = await _context.ProjectWBS.Where(s => s.ProjectId == projectId).Include(s => s.Childeren).ToListAsync();

            var workSteps = await _context.WorkPackageStep.Where(s => s.WorkPackageId == wId).OrderBy(s => s.Id)
                .ToDictionaryAsync(x => x.Id, s => s.Title);

            var wokWbs = wbsItems.FirstOrDefault(s => s.TargetId == wId && s.Type == WBSType.WorkPackage);
            if (wokWbs == null) return null;

            var pHelper = new ProgressHelper();
            var wChilds = new List<ProjectWBS>();
            pHelper.geChilds(wokWbs, wChilds);
            var locations = wChilds.Where(s => s.Type == WBSType.Location).ToArray();

            var subsystemIds = await _context.ProjectSubSystems
                .Where(s => s.ProjectSystem.ProjectId == projectId).Select(s => s.Id).ToArrayAsync();

            var allActivity = await _context.Activites.Where(s => subsystemIds.Contains(s.SubsytemId) && s.WorkPackageId == wId).Select(s => new ActivityListDetailsDto
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
                ValueUnitId = s.ValueUnitId,
                ValueUnitNum = s.ValueUnitNum,
                Status = s.Status,
                WorkPackageStepId = s.WorkPackageStepId
            }).ToListAsync();

            var viewModel = new List<WFReportList>();

            pHelper.CalculateActivityWFRelatedToWBSLastParent(wbsItems, allActivity, WBSType.Location);

            foreach (var loc in locations)
            {
                var item = new WFReportList();

                item.LocationId = loc.TargetId;
                foreach (var wst in workSteps)
                {
                    var lstItems = new WFReport();

                    lstItems.Name = wst.Value;
                    lstItems.Id = wst.Key;

                    var desActivity = allActivity.Where(s => s.LocationId == loc.TargetId &&
                        s.WorkPackageStepId == wst.Key);

                    if (desActivity.Any())
                    {
                        float calwf = desActivity.Sum(s => s.WeightFactor);
                        var doneActivity = desActivity.Where(s => s.Status == ActivityStatus.Done);
                        lstItems.Wf = desActivity.Sum(s => s.WeightFactor) * 100;
                        lstItems.Progress = doneActivity.Sum(s => (s.Progress / 100) * s.WeightFactor) * 100;
                        lstItems.WfCalculate = true;
                        lstItems.PCT = doneActivity.Sum(s => s.Progress * s.WeightFactor) / calwf;
                    }
                    item.Items.Add(lstItems);
                }
                viewModel.Add(item);
            }

            var resultQuery = viewModel.SelectMany(s => s.Items);

            var groupedItems = resultQuery.GroupBy(s => s.Id);
            var itemLast = new WFReportList();

            foreach (var gr in groupedItems)
            {
                var lstItems = new WFReport();
                lstItems.Name = gr.First().Name;
                lstItems.Wf = gr.Sum(s => s.Wf) / locations.Count();
                lstItems.Progress = gr.Sum(s => s.Progress) / locations.Count();
                lstItems.PCT = gr.Sum(s => s.PCT) / locations.Count();

                itemLast.Items.Add(lstItems);
                itemLast.LocationId = 0;
            }

            viewModel.Add(itemLast);
            return viewModel;
        }

        //wbs report
        public async Task<List<WBSExcelDto>> GetWBSExportData(Guid projectId, bool toActivity,bool calcProgress)
        {
            var allwbsItems = await _context.ProjectWBS.Where(s => s.ProjectId == projectId)
                .Include(s => s.Parent).Include(s => s.Childeren).ToListAsync();

            var sbIds = allwbsItems.Where(s => s.Type == WBSType.SubSystem).Select(s => s.TargetId).Distinct().ToArray();

            var activityes = await _context.Activites.Where(s => sbIds.Contains(s.SubsytemId))
                .Select(s => new ActivityListDetailsDto
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
                    ActivityCode=s.ActivityCode
                }).ToListAsync();

            var pHelper = new ProgressHelper();

            var wbsItems = pHelper.CalculateProgress(allwbsItems, activityes, toActivity).ToList();

            var projectItem = wbsItems.Where(s => s.Type == WBSType.Project).First();
            pHelper.WBSActivityRecovery(projectItem);
            if (calcProgress)
            {
                pHelper.progressRecovery(projectItem);
            }
            else
            {
                pHelper.WightFactorRecovery(projectItem);
            }

            var manualPr = wbsItems.Where(s => s.CalculationType == WfCalculationType.Manual);
            pHelper.ManualprogressRecovery(manualPr);

            var models =new List<WBSExcelDto>();

            int lastCounter = 1;

            wbsItems.ForEach(s =>
            {
                var item = wbsItems.First(o => o.Id == s.Id);
                int parentLevel = pHelper.ParetntCounter(s);
                int index = wbsItems.IndexOf(item) + 2;
                int counter = pHelper.childCount(s);
                lastCounter = counter + lastCounter;

                models.Add(new WBSExcelDto
                {
                    Name = s.Name,
                    WBSCode = s.WBSCode,
                    WF = s.WF,
                    Progress = s.Progress,
                    ParentLevel = parentLevel,
                    ActivityCount = s.ActivityCount,
                    LastCounter = lastCounter,
                    Type=s.Type
                });
            });

            return models;
        }

        //task report
        public async Task<ClearPunchReportDto> GetDailyTaskFormTypeReport(DateTime fromDate, DateTime toDate, Guid projectid,int workId)
        {
            var viewModel = new ClearPunchReportDto();
            var desciplines = await _context.Desciplines.Select(x => new { x.Id, x.Name }).ToListAsync();
            var sbIds = await _context.ProjectSubSystems.Where(s => s.ProjectSystem.ProjectId == projectid).Select(s => s.Id).ToArrayAsync();
            var Tasks = await _context.Activites.Where(ac=>ac.WorkPackageId==workId && (sbIds.Contains(ac.SubsytemId))
            && ac.Status!=ActivityStatus.Delete)
            .Select(x => new { x.FormDictionaryId,x.FormDictionary.Type, x.DesciplineId, x.ActualStartDate,x.Status }).ToListAsync();

            var formTypes = Enum.GetValues(typeof(Common.FormDictionaryType)).Cast<Common.FormDictionaryType>();

            foreach (var desipline in desciplines)
            {
                var pu = Tasks.Where(s => s.DesciplineId == desipline.Id);
                var performed = pu.Where(s =>(s.Status==ActivityStatus.Done) &&
                s.ActualStartDate.HasValue && (s.ActualStartDate.Value.Date > fromDate.Date && s.ActualStartDate.Value.Date <= toDate.Date));
                var remain = pu.Where(s => s.Status == ActivityStatus.Ongoing || s.Status == ActivityStatus.NotStarted || s.Status == ActivityStatus.Reject);

                var ginItem = new ClearPunchDetailsDto
                {
                    DesciplineName = desipline.Name,
                    DesciplineId = desipline.Id,
                };

                foreach (var ptype in formTypes)
                {
                    var kut = pu.Where(s => s.Type == ptype);
                    var klt = performed.Where(s => s.Type == ptype);
                    var krt = remain.Where(s => s.Type == ptype);

                    ginItem.Totals.Add(kut.Count());
                    ginItem.ClearPunch.Add(klt.Count());
                    ginItem.Remain.Add(krt.Count());
                }

                viewModel.Items.Add(ginItem);
            }

            viewModel.PunchType = formTypes.Select(x => new PunchTypeServices.PunchTypeListDto
            {
                Name = x.ToString(),
            });

            return viewModel;
        }

        //punch report


        public async Task<ClearPunchReportDto> GetDailyPunchClearReport(DateTime fromDate,DateTime toDate,Guid projectid)
        {
            var viewModel = new ClearPunchReportDto();
            var desciplines =await _context.Desciplines.Select(x => new { x.Id, x.Name }).ToListAsync();
            var punches = await _context.Punchs.Select(x => new { x.PunchTypeId,x.Activity.DesciplineId,x.ClearDate}).ToListAsync();

            var punchTypes =await _context.PunchTypes.Where(s => s.ProjectId == projectid)
                .Select(x => new PunchTypeServices.PunchTypeListDto
                {
                    Id=x.Id,
                    Name=x.Name,
                }).OrderBy(s => s.Id).ToListAsync();

            foreach(var desipline in desciplines)
            {
                var pu = punches.Where(s => s.DesciplineId == desipline.Id);
                var clearPunces = pu.Where(s => s.ClearDate.HasValue && (s.ClearDate.Value.Date>fromDate.Date && s.ClearDate.Value.Date<=toDate.Date) );
               
                var ginItem = new ClearPunchDetailsDto
                {
                    DesciplineName = desipline.Name,
                    DesciplineId = desipline.Id,
                };
                ginItem.Totals.Add(pu.Count());
                ginItem.ClearPunch.Add(clearPunces.Count());
                foreach(var ptype in punchTypes)
                {
                    var kut = pu.Where(s => s.PunchTypeId == ptype.Id);
                    var klt = clearPunces.Where(s => s.PunchTypeId == ptype.Id);
                    ginItem.Totals.Add(kut.Count());
                    ginItem.ClearPunch.Add(klt.Count());
                }

                viewModel.Items.Add(ginItem);
            }

            if (punchTypes.Any())
            {
                punchTypes.Insert(0, new PunchTypeServices.PunchTypeListDto { Id = -1, Name = "Sum" });
            }

            viewModel.PunchType = punchTypes;

            return viewModel;
        }

        public async Task<PunchCategoryReportDto> GetPunchCategoryReport(DateTime fromDate, DateTime toDate, Guid projectid)
        {
            var viewModel = new PunchCategoryReportDto();
            var categoryies = await _context.PunchCategories.Where(s => s.ProjectId == projectid)
                .Select(s => new PunchCategoryDetailsReportDto
            {
                Name=s.Name,
                Id=s.Id
            }).ToListAsync();

            var punches = await _context.Punchs.Where(s=>!s.ClearDate.HasValue &&
            (s.OrginatedDate.Date>fromDate.Date && s.OrginatedDate.Date<=toDate.Date))
                .Select(x => new { x.PunchTypeId, x.CategoryId, x.Activity.DesciplineId, x.Activity.Condition }).ToListAsync();

            var desciplines = await _context.Desciplines.Select(s => new { s.Name, s.Id }).OrderBy(s=>s.Id).ToListAsync();
            var pTypes = await _context.PunchTypes.Where(s => s.ProjectId == projectid)
                .Select(s => new { s.Id, s.Name }).OrderBy(s => s.Id).ToListAsync();

            foreach(var cat in categoryies)
            {
                var pcat = punches.Where(s => s.CategoryId == cat.Id);
                var pHold = pcat.Where(s => s.Condition == ActivityCondition.Hold);
                var item = new PunchCategoryDetailsReportDto { Name=cat.Name,Id=cat.Id};

                item.Totals.Add(new Tuple<int, int>(pcat.Count() - pHold.Count(), pHold.Count()));

                foreach (var des in desciplines)
                {
                    var pdes = pcat.Where(s => s.DesciplineId == des.Id);
                    var pdetails = new List<Tuple<string,int, int>>();
                    foreach (var pType in pTypes)
                    {
                        var pdestype = pdes.Where(s => s.PunchTypeId == pType.Id);
                        var pdestypeHold = pdestype.Where(s => s.Condition == ActivityCondition.Hold);

                        pdetails.Add(new Tuple<string,int, int>(pType.Name,pdestype.Count()-pdestypeHold.Count(),pdestypeHold.Count()));
                    }
                    item.PunchDetails.Add(new KeyValuePair<string, List<Tuple<string, int, int>>>(des.Name,pdetails));
                }

                viewModel.Categories.Add(item);
            }

            return viewModel;
        }

        //status report
        public async Task<List<StatusReportDto>> GetStatusReport(Guid projectId,int workId)
        {
            var viewModel = new List<StatusReportDto>();

            var subsystems = await _context.ProjectSubSystems.Where(s => s.ProjectSystem.ProjectId == projectId).ToListAsync();
            var sbIds = subsystems.Select(s => s.Id).ToArray();

            var descipline = await _context.Desciplines.ToListAsync();
            var location = await _context.LocationTypes.ToListAsync();

            var Tasks = await _context.Activites.Where(ac => ac.WorkPackageId == workId && (sbIds.Contains(ac.SubsytemId))
            && ac.Status != ActivityStatus.Delete)
            .Select(x => new {x.Id, x.SubsytemId, x.DesciplineId, x.Status }).ToListAsync();

            var pTypes = await _context.PunchTypes.Where(s => s.WorkPackages.Any(w => w.Precentage > 20 && w.WorkPackageId == workId))
                            .OrderBy(s => s.Id).FirstOrDefaultAsync();

            var punchs = await _context.Punchs.Where(s => s.PunchTypeId==pTypes.Id).Select(s => new
            {
                s.ActivityId,
                s.PunchTypeId
            }).ToListAsync();

            foreach(var sb in subsystems)
            {
                var sbTsk = Tasks.Where(s => s.SubsytemId == sb.Id);
                if (sbTsk.Any())
                {
                    var item = new StatusReportDto();
                    item.Priority = sb.PriorityNo + (sb.SubPriorityNo!=null?"-" + sb.SubPriorityNo:"");
                    item.SubSystemCode = sb.Code;
                    item.SubSystemDesc = sb.Description;

                    var gsbTsk = sbTsk.GroupBy(s => s.DesciplineId);
                    foreach(var gb in gsbTsk)
                    {
                        var dItem = new StatusReportDetailDto();
                        var des = descipline.FirstOrDefault(s => s.Id == gb.Key);
                        dItem.Descipline = des.Name;

                        var tIds = gb.Select(s => s.Id);

                        if (pTypes!=null)
                        {
                            var pubydes = punchs.Where(s => tIds.Contains(s.ActivityId) && s.PunchTypeId== pTypes.Id);
                            dItem.RemainPunchNum1 = pubydes.Count();
                            dItem.RemainPunchDesc1 ="Remain Punch "+ pTypes.Name;
                        }

                        var rTasks = gb.Where(s => s.Status != ActivityStatus.Done);
                        dItem.RemainSheet = rTasks.Count();
                        if(dItem.RemainSheet>0 || dItem.RemainPunchNum1>0 )
                        {
                            dItem.StatusDesc = "Not Ok";
                        }
                        else
                        {
                            dItem.StatusDesc = "Ok";
                        }

                        item.StatusDetails.Add(dItem);
                    }
                    item.StatusDesc = "Not Ready";
                    if (item.StatusDetails.All(s => string.Equals(s.StatusDesc, "OK", StringComparison.OrdinalIgnoreCase)))
                    {
                        item.StatusDesc = "Ready";
                    }
                    viewModel.Add(item);
                }
            }
            return viewModel;
        }
    }
}
