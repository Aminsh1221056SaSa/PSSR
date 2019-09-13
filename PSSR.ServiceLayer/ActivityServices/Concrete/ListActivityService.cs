using AutoMapper;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.EntityFrameworkCore;
using PSSR.Common;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DataLayer.EfCode;
using PSSR.DataLayer.QueryObjects;
using PSSR.ServiceLayer.ActivityServices.QueryObjects;
using PSSR.ServiceLayer.ProjectServices;
using PSSR.ServiceLayer.Utils;
using PSSR.ServiceLayer.Utils.ProgressHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.ActivityServices.Concrete
{
    public class ListActivityService
    {
        private readonly EfCoreContext _context;
        private readonly IMapper _mapper;

        public ListActivityService(EfCoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ListActivityService(EfCoreContext context) : this(context, null) { }

        public async Task<string> GetActivityDocumentFilePath(long activityDocId)
        {
           return await _context.ActivityDocuments.Where(s => s.Id == activityDocId)
                .Select(s => s.FilePath).SingleOrDefaultAsync();
        }

        public async Task<long> GetActivityNextCode()
        {
            var query = _context.Activites;

            if (query.Any())return (await query.MaxAsync(ac => ac.Id)) + 1;
            return 1;
        }
        public async Task<bool> HasMainDocument(long activityId)
        {
            return await _context.Activites.Where(s => s.Id == activityId)
                .SelectMany(s => s.ActivityDocuments).AnyAsync(s => s.Description == "Main Document");
        }

        public async Task<Dictionary<string,long>> GetAllActivityDic(Guid projectId)
        {
            var activityies =await this.GetProjectActivities(projectId);
            return activityies.DistinctBy(s=>s.ActivityCode)
                .ToDictionary(s => s.ActivityCode, s => s.Id);
        }

        public async Task<ActivityListDetailsDto> GetActivity(long id)
        {
            var activity =await _context.Activites.Where(s => s.Id == id)
                .Select(item => new ActivityListDetailsDto
            {
                ActualEndDate= item.ActualEndDate != null ? item.ActualEndDate.Value.ToString("d") : "",
                ActualMh= item.ActualMh,
                ActualStartDate= item.ActualStartDate != null ? item.ActualStartDate.Value.ToString("d") : "",
                EstimateMh = item.EstimateMh,
                PlanEndDate = item.PlanEndDate != null ? item.PlanEndDate.Value.ToString("d") : "",
                Status = item.Status,
                PlanStartDate= item.PlanStartDate != null ? item.PlanStartDate.Value.ToString("d") : "",
                Id = item.Id,
                ActivityCode=item.ActivityCode,
                Progress= item.Progress,
                TagDescription= item.TagDescription,
                TagNumber= item.TagNumber,
                ValueUnitId= item.ValueUnitId,
                DesciplineId=item.DesciplineId,
                ValueUnitNum= item.ValueUnitNum,
                WeightFactor= item.WeightFactor,
                LocationId=item.LocationId,
                SubsytemId=item.SubsytemId,
                WorkPackageId=item.WorkPackageId,
                Condition=item.Condition,
                DesciplineName=item.Descipline.Name,
                LocationName=item.Location.Title,
                WorkPackageName=item.WorkPackage.Name,
                SubSystemName=item.SubSystem.Code,
                SystemName=item.SubSystem.ProjectSystem.Code,
                SystemdId=item.SubSystem.ProjectSystemId,
                FormDictionaryId = item.FormDictionaryId,
                FormCode=item.FormDictionary.Code,
                FormDescription=item.FormDictionary.Description,
                FormType=item.FormDictionary.Type.ToString(),
                WorkPackageStepId=item.WorkPackageId,
                WorkPackageStepName=item.WorkPackageStep.Title
                }).DefaultIfEmpty().SingleOrDefaultAsync();
            return activity;
        }

        public async Task<List<ActivityStatusHistoryListDto>> GetStatusHistory(long activityId)
        {
            return await _context.Activites.Where(s => s.Id == activityId).SelectMany(s => s.StatusHistory)
                .Select(s => new ActivityStatusHistoryListDto
            {
                ActivityId=s.ActivityId,
                CreateDate=s.CreateDate.ToString("dddd, dd MMMM yyyy"),
                Description=s.Description,
                HoldBy=s.HoldBy,
                Id=s.Id
            }).ToListAsync();
        }

        public async Task<List<ActivityDocumentListDto>> GetDocuments(long activityId)
        {
            return await _context.Activites.Where(s => s.Id == activityId).SelectMany(s => s.ActivityDocuments)
                .Select(s => new ActivityDocumentListDto
                {
                    Id=s.Id,
                    ActivityId = s.ActivityId,
                    CreatedDate = s.CreatedDate.ToString("dddd, dd MMMM yyyy"),
                    Description = s.Description,
                    PunchId=s.PunchId,
                    PunchCode=s.Punch.Code
                }).DefaultIfEmpty().ToListAsync();
        }

        public async Task<IEnumerable<ProjectWBSListDto>> GetActivityWBSTree(ActivityListDetailsDto activity,Guid projectId)
        {
            var wbsItems = await _context.ProjectWBS
                .Where(s => s.ProjectId == projectId).Include(s => s.Parent).Include(s => s.Childeren).ToListAsync();

            if(!wbsItems.Any())
            {
                return null;
            }

            var project = wbsItems.Where(s => s.Type == WBSType.Project).FirstOrDefault();
            var workPackage = wbsItems.Where(s => s.TargetId == activity.WorkPackageId).FirstOrDefault();
            if (workPackage == null)
            {
                return null;
            }

            var lstChilds = new Dictionary<WBSType, long>();
            lstChilds.Add(WBSType.Location, activity.LocationId);
            lstChilds.Add(WBSType.Descipline, activity.DesciplineId);
            lstChilds.Add(WBSType.System, activity.SystemdId);
            lstChilds.Add(WBSType.SubSystem, activity.SubsytemId);

            var lstWbsTree = new List<ProjectWBS>();
            new ProgressHelper().getActivityWbsTree(workPackage, lstChilds, lstWbsTree);
            lstWbsTree.Add(project);
            lstWbsTree.Add(workPackage);

            return (from c in lstWbsTree.OrderBy(s=>s.Id)
                    select new ProjectWBSListDto
                    {
                        Id=c.Id,
                        Name=c.Name,
                        ParentId=c.ParentId,
                        WBSCode=c.WBSCode,
                        WF=c.WF,
                        Type=c.Type
                    });
        }

        public async Task<List<Activity>> GetProjectActivities(Guid projectId)
        {
            var subSystemIds = await _context.ProjectSystems.Where(s => s.ProjectId == projectId)
                .SelectMany(s => s.ProjectSubSystems).Select(s => s.Id).ToArrayAsync();
            var activityies = await _context.Activites.Where(s => subSystemIds.Contains(s.SubsytemId)).ToListAsync();
            return activityies;
        }

        public async Task<List<Activity>> GetProjectActivitiesIncludePunches(Guid projectId)
        {
            var subSystemIds = await _context.ProjectSystems.Where(s => s.ProjectId == projectId)
                .SelectMany(s => s.ProjectSubSystems).Select(s => s.Id).ToArrayAsync();
            var activityies = await _context.Activites.Where(s => subSystemIds.Contains(s.SubsytemId))
                .Include(s=>s.Punchs).ToListAsync();
            return activityies;
        }

        public async Task<IEnumerable<PlanActivityDto>> GetProjectPlanActivity(int workPackageId, int locationId, long subsystemId
            , int decsiplineId, long formId)
        {
            var activities =await _context.Activites.Where(s => s.WorkPackageId == workPackageId && s.LocationId == locationId
                && s.SubsytemId == subsystemId && s.DesciplineId == decsiplineId && s.FormDictionaryId == formId).ToListAsync();
            return activities.Select(item => new PlanActivityDto
              {
                  Id = item.Id,
                  WF = item.WeightFactor,
                  Status = item.Status,
                  ActivityCode=item.ActivityCode,
                  EndDate=item.PlanEndDate.Value.ToString("d"),
                  StartDate=item.PlanStartDate.Value.ToString("d"),
                  StartTime=item.PlanStartDate.Value.ToString("hh:mm tt", CultureInfo.InvariantCulture),
                  EndTime= item.PlanEndDate.Value.ToString("hh:mm tt", CultureInfo.InvariantCulture),
            });
        }

        //get manipulation data

        public Task<IQueryable<ActivityListDto>> SortFilterPage
          (ActivitySortFilterPageOptions options, Guid projectId)
        {
            return Task.Run(() =>
            {
                var systemIds = _context.ProjectSystems.Where(s => s.ProjectId == projectId).Select(s => s.Id).ToArray();
                var activityQuery = _context.Activites.AsNoTracking()
                .Where(item => item.Status!=ActivityStatus.Delete && systemIds.Contains(item.SubSystem.ProjectSystemId) &&
                (
                item.TagNumber.Contains(options.QueryFilter) || string.IsNullOrWhiteSpace(options.QueryFilter)))
                .FilterActivityBy(options.FilterBy, options.FilterValue)
                .MapActivityToDto(options.WorkPackageId, options.LocationId, options.SystemId, options.SubSystemId)
                .OrderActivityBy(options.OrderByOptions);

                options.SetupRestOfDto(activityQuery);

                return activityQuery.Page(options.PageNum - 1,
                                       options.PageSize);
            });
        }

        public Task<IQueryable<ActivityListDto>> SortFilterByDesciplineWorkPackagePage
          (ActivitySortFilterPageOptions options, int workId, int desId, Guid projectId)
        {
            return Task.Run(() =>
            {
                var systemIds = _context.ProjectSystems.Where(s => s.ProjectId == projectId).Select(s => s.Id).ToArray();
                var activityQuery = _context.Activites
               .AsNoTracking()
               .Where(item => item.Status != ActivityStatus.Delete && systemIds.Contains(item.SubSystem.ProjectSystemId) &&
               item.WorkPackageId == workId && item.DesciplineId == desId &&
               (item.TagNumber.Contains(options.QueryFilter) || string.IsNullOrWhiteSpace(options.QueryFilter)))
               .FilterActivityBy(options.FilterBy, options.FilterValue)
               .MapActivityToDtoWithDateTime()
               .OrderActivityBy(options.OrderByOptions);

                options.SetupRestOfDto(activityQuery);

                return activityQuery.Page(options.PageNum - 1,
                                       options.PageSize);
            });
        }

        public async Task<JqueryDataTablesPagedResults<ActivityListDataTableDto>> SortFilterPageDataTable
         (JqueryDataTablesParameters table, Guid projectId)
        {
            var systemIds = _context.ProjectSubSystems.Where(s => s.ProjectSystem.ProjectId == projectId).Select(s => s.Id).ToArray();
            var query = _context.Activites.Where(s =>s.Status!=ActivityStatus.Delete && systemIds.Contains(s.SubsytemId));

            query = new SortOptionsProcessor<ActivityListDataTableDto, Activity>().Apply(query, table);
            query = new SearchOptionsProcessor<ActivityListDataTableDto, Activity>().Apply(query, table.Columns);

            var size = await query.CountAsync();

            var items = await query
                .AsNoTracking()
                .Skip((table.Start / table.Length) * table.Length)
                .Take(table.Length)
               .MapActivityToDtoDataTable().ToListAsync();

            return new JqueryDataTablesPagedResults<ActivityListDataTableDto>
            {
                Items =items,
                TotalSize = size
            };
        }

        public async Task<JqueryDataTablesPagedResults<ActivityListExcelDataTableDto>> GetExcelExportDataTableAsync(JqueryDataTablesParameters table, Guid projectId)
        {
            var subSystems =await _context.ProjectSubSystems.Where(s => s.ProjectSystem.ProjectId == projectId)
                .Include(s=>s.ProjectSystem).ToListAsync();
            var subIds = subSystems.Select(s => s.Id).ToArray();

            var workpackages = await _context.ProjectRoadMaps.ToListAsync();
            var locations = await _context.LocationTypes.ToListAsync();
            var forms = await _context.FormDictionaries.ToListAsync();
            var desciplines = await _context.Desciplines.ToListAsync();
            var workSteps = await _context.WorkPackageStep.ToListAsync();

            var query = _context.Activites.Where(s => s.Status != ActivityStatus.Delete && subIds.Contains(s.SubsytemId));

            query = new SortOptionsProcessor<ActivityListDataTableDto, Activity>().Apply(query, table);
            query = new SearchOptionsProcessor<ActivityListDataTableDto, Activity>().Apply(query, table.Columns);

            var items = await query.AsNoTracking()
               .MapActivityToDtoDataTable().ToListAsync();

            foreach(var item in items)
            {
                var subsystem = subSystems.Single(s => s.Id == item.SubsytemId);
                item.SubSystem = subsystem.Code;
                item.System = subsystem.ProjectSystem.Code;

                var workpackage = workpackages.Find(s => s.Id == item.WorkPackageId);
                item.WorkPackage = workpackage.Name;

                var location = locations.Find(s => s.Id == item.LocationId);
                item.Location = location.Title;

                var form = forms.Find(s => s.Id == item.FormDictionaryId);
                item.FormDictionary = form.Code;
                item.FormType = form.Type.ToString();

                var workStep = workSteps.Find(s => s.Id == item.WorkPackageStepId);
                item.WorkPackageStep = workStep.Title;

                var descipline = desciplines.Find(s => s.Id == item.DesciplineId);
                item.Descipline = descipline.Name;

            }
            return new JqueryDataTablesPagedResults<ActivityListExcelDataTableDto>
            {
                Items = items.MapActivityToExcelDtoDataTable()
            };
        }
    }
}
