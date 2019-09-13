using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PSSR.Common;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ActivityServices;
using PSSR.ServiceLayer.Utils;
using PSSR.ServiceLayer.Utils.ProgressHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.ProjectServices.Concrete
{
    public class ListWBSService
    {
        private readonly EfCoreContext _context;
        private readonly IMapper _mapper;

        public ListWBSService(EfCoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string GetProjectWBSNextChildCode(long wbsParentId, string parentWbsCode)
        {
            var lastChild = _context.ProjectWBS.Where(s => s.ParentId == wbsParentId).LastOrDefault();

            if (lastChild != null)
            {
                var temp = lastChild.WBSCode.Split('-');
                var templast = temp.Last();
                int lastCode = 1;
                int.TryParse(templast, out lastCode);
                return parentWbsCode + $"-{(lastCode+1)}";
            }
            else
            {
                return parentWbsCode + $"-1";
            }
        }

        public bool hasWbSChildDublicate(long parentId, WBSType type, string name)
        {
            var parent = _context.ProjectWBS.Where(s => s.Id == parentId).Include(s => s.Childeren).SingleOrDefault();
            if (parent == null)
            {
                return false;
            }

            if (parent.Childeren.Any(s => s.Name == name))
            {
                return true;
            }

            return false;
        }

        public bool wbsHasAnyActivity(long targetId,WBSType wTyep,Guid projectId)
        {
            switch (wTyep)
            {
                case WBSType.WorkPackage:
                   return _context.Activites.Any(s => s.WorkPackageId == targetId && 
                   s.SubSystem.ProjectSystem.ProjectId == projectId);
                case WBSType.Location:
                    return _context.Activites.Any(s => s.LocationId == targetId &&
                    s.SubSystem.ProjectSystem.ProjectId == projectId);
                case WBSType.Descipline:
                    return _context.Activites.Any(s => s.DesciplineId == targetId &&
                    s.SubSystem.ProjectSystem.ProjectId == projectId);
                case WBSType.System:
                    return _context.Activites.Any(s => s.SubSystem.ProjectSystemId == targetId);
                case WBSType.SubSystem:
                    return _context.Activites.Any(s => s.SubsytemId == targetId);
                default:
                    return false;
            }
        }

        public bool WBSNodeHasAnyChild(long wbsId)
        {
            return _context.ProjectWBS.Where(s => s.Id == wbsId).Any(s => s.Childeren.Any());
        }

        public bool IsValidWBSWf(float newWf, long wbsparentId, long currentId)
        {
            var dbWf = _context.ProjectWBS.Where(s => s.ParentId == wbsparentId && s.Id != currentId).Sum(s => s.WF);

            float sumWf = dbWf + newWf;

            return sumWf > 100 ? false : true;
        }

        public async Task<bool> hasParentSameType(WBSType type, Guid projectId, long parentId)
        {
            var items = await _context.ProjectWBS.Where(s => s.ProjectId == projectId)
                   .Include(s => s.Childeren).ToListAsync();

            var parent = items.First(s => s.Id == parentId);
            var pHelper = new ProgressHelper();
            var wbsParents = new List<WBSType>();
            pHelper.ParentRicovery(parent, wbsParents);
            if (wbsParents.Any(s => s == type))
            {
                return true;
            }
            return false;
        }

        public async Task<List<ProjectWBSListDto>> GetWbsTargetChilderen(Guid projectId,long[] targetIds, WBSType parentType)
        {
            return await _context.ProjectWBS.Where(s =>s.ProjectId==projectId && targetIds.Contains(s.TargetId) && s.Type == parentType)
                .SelectMany(s => s.Childeren).Select(s => new ProjectWBSListDto
                {
                    Id = s.TargetId,
                    Name = s.Name,
                    ParentId = s.Parent.TargetId
                }).ToListAsync();
        }
        public async Task<List<ProjectWBSListDto>> GetProjectWBSRelated(Guid projectId, WBSType relatedType,long? parentId,int locationId)
        {
            if (locationId > 0)
            {
                return await _context.ProjectWBS.Where(s => s.ProjectId == projectId && 
                s.Type == relatedType && s.TargetId == locationId && (s.Parent.TargetId == parentId || !parentId.HasValue))
             .Select(s => new ProjectWBSListDto
             {
                 Id = s.Id,
                 TargetId = s.TargetId,
                 Name = s.Name,
                 ParentId = s.Parent.TargetId
             }).ToListAsync();
            }
            else
            {
                return await _context.ProjectWBS.Where(s => s.ProjectId == projectId && s.Type == relatedType &&
               (s.Parent.TargetId == parentId || !parentId.HasValue))
             .Select(s => new ProjectWBSListDto
             {
                 Id = s.Id,
                 TargetId = s.TargetId,
                 Name = s.Name,
                 ParentId = s.Parent.TargetId
             }).ToListAsync();
            }
        }

        public async Task<IEnumerable<ProjectWBSListDto>> GetProjectWBSTree(Guid projectId)
        {
            var items = await _context.ProjectWBS.Where(s => s.ProjectId == projectId).Include(s => s.Childeren).ToListAsync();

            var itemsDto = _mapper.Map<IEnumerable<ProjectWBS>, IEnumerable<ProjectWBSListDto>>(items.Where(s => s.Parent == null));

            return itemsDto;
        }


        public async Task<IEnumerable<ProjectWBSListDto>> GetWBSProgress(Guid projectId, bool toProgress)
        {
            var allwbsItems = await _context.ProjectWBS.Where(s => s.ProjectId == projectId)
                .Include(s => s.Parent).Include(s => s.Childeren).ToListAsync();
            var sbIds = allwbsItems.Where(s => s.Type == WBSType.SubSystem)
                .Select(s => s.TargetId).Distinct().ToArray();

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
                    ValueUnitId = s.ValueUnitId,
                    ValueUnitNum = s.ValueUnitNum,
                    Status = s.Status,
                    WorkPackageStepId = s.WorkPackageStepId
                }).ToListAsync();

            var pHelper = new ProgressHelper();
            var wbsItems = pHelper.CalculateProgress(allwbsItems, activityes, false);

            var projectItem = wbsItems.Where(s => s.Type == WBSType.Project).First();
            pHelper.WBSActivityRecovery(projectItem);

            if (toProgress)
            {
                pHelper.progressRecovery(projectItem);
                var manualPr = wbsItems.Where(s => s.CalculationType == WfCalculationType.Manual);
                pHelper.ManualprogressRecovery(manualPr);

                var itemsDto = _mapper.Map<IEnumerable<ProjectWBS>,
                    IEnumerable<ProjectWBSListDto>>(wbsItems.Where(s => s.Parent == null));
                return itemsDto;
            }
            else
            {
                var itemsDto = _mapper.Map<IEnumerable<ProjectWBS>, IEnumerable<ProjectWBSListDto>>(allwbsItems.Where(s => s.Parent == null));

                return itemsDto;
            }
        }

        public async Task<DataTable> CalculateActivityWBSOnTableAction(ActivityListDetailsDto activity, Guid projectId)
        {
            var wbsItems = await _context.ProjectWBS
                .Where(s => s.ProjectId == projectId).Include(s => s.Parent).Include(s => s.Childeren).ToListAsync();

            var project = wbsItems.Where(s => s.Type==WBSType.Project).FirstOrDefault();
            if (project == null)
            {
                return null;
            }

            var lstChilds = new Dictionary<WBSType, long>();
            lstChilds.Add(WBSType.WorkPackage, activity.WorkPackageId);
            lstChilds.Add(WBSType.Location, activity.LocationId);
            lstChilds.Add(WBSType.Descipline, activity.DesciplineId);
            lstChilds.Add(WBSType.System, activity.SystemdId);
            lstChilds.Add(WBSType.SubSystem, activity.SubsytemId);

            var lstWbsTree = new List<ProjectWBS>();
            new ProgressHelper().getActivityWbsTree(project, lstChilds, lstWbsTree);

            var wIds = lstWbsTree.Where(s => s.Type == WBSType.WorkPackage).Select(s => s.TargetId).ToArray();
            var sbIds = lstWbsTree.Where(s => s.Type == WBSType.SubSystem).Select(s => s.TargetId).ToArray();
            var desIds = lstWbsTree.Where(s => s.Type == WBSType.Descipline).Select(s => s.TargetId).ToArray();
            var locIds = lstWbsTree.Where(s => s.Type == WBSType.Location).Select(s => s.TargetId).ToArray();
            var systemIds = lstWbsTree.Where(s => s.Type == WBSType.System).Select(s => s.TargetId).ToArray();

            var subIds = await _context.ProjectSystems.Where(s => s.ProjectId == projectId)
               .SelectMany(s => s.ProjectSubSystems).Select(s => s.Id).ToArrayAsync();
            var query = _context.Activites.Where(s => subIds.Contains(s.SubsytemId)
                                  && s.Status != ActivityStatus.Delete).AsQueryable();

            if (lstWbsTree.Any(s => s.Type == WBSType.WorkPackage))
            {
                query = query.Where(s => wIds.Contains(s.WorkPackageId));
            }

            if (lstWbsTree.Any(s => s.Type == WBSType.Location))
            {
                query = query.Where(s => locIds.Contains(s.LocationId));
            }

            if (lstWbsTree.Any(s => s.Type == WBSType.Descipline))
            {
                query = query.Where(s => desIds.Contains(s.DesciplineId));
            }

            if (lstWbsTree.Any(s => s.Type == WBSType.System))
            {
                query = query.Where(s => systemIds.Contains(s.SubSystem.ProjectSystemId));
            }

            if (lstWbsTree.Any(s => s.Type == WBSType.SubSystem))
            {
                query = query.Where(s => sbIds.Contains(s.SubsytemId));
            }

            var items = await query.ToListAsync();

            var dt = new DataTableHelper().getUpdateActivityWFTable();
            if (items != null && items.Any())
            {
                int gCount = items.Count();
                var gbyvaluUnit = items.GroupBy(s => s.ValueUnitId);
                foreach (var gv in gbyvaluUnit)
                {
                    float sumUnit = gv.Sum(s => s.ValueUnitNum);
                    var cCount = gv.Select(s => s).Count();

                    float formolate = (float)cCount / gCount;

                    float temp = (float)100 / sumUnit;

                    foreach (var acnext in gv)
                    {
                        float th = (temp * acnext.ValueUnitNum) * formolate;
                        dt.Rows.Add(acnext.Id, th);
                    }
                }
            }

            return dt;
        }

        public async Task<DataTable> calculateWFForAllActivity(Guid projectId)
        {
            var distinectedType = await _context.ProjectWBS
                .Where(s => s.ProjectId == projectId).Include(s => s.Parent)
                .Include(s => s.Childeren).ToListAsync();

            var subIds = await _context.ProjectSystems.Where(s => s.ProjectId == projectId)
                .SelectMany(s => s.ProjectSubSystems).Select(s => s.Id).ToArrayAsync();

            var allActivity = await _context.Activites.Where(s => subIds.Contains(s.SubsytemId)
                                  && s.Status != ActivityStatus.Delete).Select(s => new ActivityListDetailsDto
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
                                      ValueUnitId=s.ValueUnitId,
                                      ValueUnitNum=s.ValueUnitNum
                                  }).ToListAsync();

            var dt = new DataTableHelper().getUpdateActivityWFTable();

            var destinictLastItems = distinectedType.Where(s => !s.Childeren.Any()).ToList();

            if (!destinictLastItems.Any())
            {
                return null;
            }
            var pHelper = new ProgressHelper();
            dt = pHelper.CalculateActivityWeithtFactor(destinictLastItems, allActivity, dt);
            return dt;
        }

        public async Task<DataTable> CalculateWFForAllWBS(Guid projectId,bool allwbsItem)
        {
            var distinectedType = await _context.ProjectWBS
                .Where(s => s.ProjectId == projectId).Include(s => s.Parent)
                .Include(s => s.Childeren).ToListAsync();

            var subIds = await _context.ProjectSystems.Where(s => s.ProjectId == projectId)
               .SelectMany(s => s.ProjectSubSystems).Select(s => s.Id).ToArrayAsync();

            var allActivity = await _context.Activites.Where(s => subIds.Contains(s.SubsytemId)
                                  && s.Status != ActivityStatus.Delete).Select(s => new ActivityListDetailsDto
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
                                      ValueUnitNum = s.ValueUnitNum
                                  }).ToListAsync();

            var dt = new DataTableHelper().getUpdateWBSWFTable();

            var pHelper = new ProgressHelper();
            if (!allwbsItem)
            {
                var destinictnowf = distinectedType.Where(s => s.WF <= 0).ToList();
                dt = pHelper.CalculateWeightForWBS(destinictnowf, allActivity, dt);
            }
            else
            {
                dt = pHelper.CalculateWeightForWBS(distinectedType, allActivity, dt);
            }
            return dt;
        }

        public async Task<IEnumerable<ProjectWBSListDto>> GetProjectWBSActivityTree(Guid projectId, long parentId)
        {
            var items = await _context.ProjectWBS.Where(s => s.ProjectId == projectId)
                .Include(s => s.Childeren).ToListAsync();

            var sbIds = await _context.ProjectSubSystems.Where(s => s.ProjectSystem.ProjectId == projectId)
                .Select(s => s.Id).ToArrayAsync();

            var lastChild = items.Single(h => h.Id == parentId);
            var pHelper = new ProgressHelper();
            var parents = new List<Tuple<long, WBSType>>();
            pHelper.ParentRicoveryToType(lastChild, parents);

            long workPackageId = 0;
            long locationId = 0;
            long desCiplineId = 0;
            long subSystemId = 0;
            long systemId = 0;

            if (parents.Any(s => s.Item2 == WBSType.WorkPackage))
            {
                workPackageId = parents.First(s => s.Item2 == WBSType.WorkPackage).Item1;
            }

            if (parents.Any(s => s.Item2 == WBSType.Location))
            {
                locationId = parents.First(s => s.Item2 == WBSType.Location).Item1;
            }

            if (parents.Any(s => s.Item2 == WBSType.Descipline))
            {
                desCiplineId = parents.First(s => s.Item2 == WBSType.Descipline).Item1;
            }

            if (parents.Any(s => s.Item2 == WBSType.System))
            {
                systemId = parents.First(s => s.Item2 == WBSType.System).Item1;
            }

            if (parents.Any(s => s.Item2 == WBSType.SubSystem))
            {
                subSystemId = parents.First(s => s.Item2 == WBSType.SubSystem).Item1;
            }

            var filteredItems = await _context.Activites.Where(s => sbIds.Contains(s.SubsytemId) && ((s.WorkPackageId == workPackageId || workPackageId == 0)
               && (s.LocationId == locationId || locationId == 0)
               && (s.FormDictionary.DesciplineLink.Any(dl => dl.DesciplineId == desCiplineId) || desCiplineId == 0)
               && (s.SubSystem.ProjectSystemId == systemId || systemId == 0)
               && (s.SubsytemId == subSystemId || subSystemId == 0))).Select(item => new ProjectWBSListDto
               {
                   Id = item.Id,
                   WF = item.WeightFactor,
                   WBSCode = item.Status.ToString(),
                   ParentId = parentId,
                   Name = item.TagNumber,
                   Progress = item.Progress,
               }).ToListAsync();

            return filteredItems;
        }
        public async Task<IEnumerable<ProjectWBSListDto>> GetProjectWBSLastChilds(Guid projectId)
        {
            var items = await _context.ProjectWBS.Where(s => s.ProjectId == projectId && !s.Childeren.Any()).Select(s => new ProjectWBSListDto
            {
                Id = s.Id,
                Name = s.Name,
                ParentId = s.ParentId,
                TargetId = s.TargetId,
                Type = s.Type,
                WBSCode = s.WBSCode,
                WF = s.WF,
                CalculationType = s.CalculationType
            }).ToListAsync();

            return items;
        }

        public async Task<IEnumerable<ProjectWBSListDto>> GetProjectWBSLastChildsSummary(Guid projectId)
        {
            var items = await _context.ProjectWBS.Where(s => s.ProjectId == projectId &&
            !s.Childeren.Any()).Select(x => new ProjectWBSListDto
            {
                Id = x.Id,
                Name = $"{x.WBSCode} ({x.Name})",
                CalculationType = x.CalculationType
            }).ToListAsync();

            return items;
        }

        public async Task<IEnumerable<ProjectWBSListDto>> GetProjectWBSAllItems(Guid projectId)
        {
            var items = await _context.ProjectWBS.Where(s => s.ProjectId == projectId &&
            (s.Type != WBSType.Project && s.Type != WBSType.Activity)).Select(s => new ProjectWBSListDto
            {
                Id = s.Id,
                Name = $"{s.Name}({ s.WBSCode})",
                ParentId = s.ParentId,
                TargetId = s.TargetId,
                Type = s.Type,
                WBSCode = s.WBSCode,
                WF = s.WF,
                CalculationType = s.CalculationType
            }).ToListAsync();

            return items;
        }

        public async Task<ProjectWBSListDto> GetProjectWBS(long wbsId)
        {
            return await _context.ProjectWBS.Where(s => s.Id == wbsId)
                .Select(c => new ProjectWBSListDto
                {
                    Id = c.Id,
                    ParentId = c.ParentId,
                    TargetId = c.TargetId,
                    Type = c.Type,
                    WBSCode = c.WBSCode,
                    WF = c.WF,
                    Name = c.Name,
                    CalculationType = c.CalculationType
                }).FirstOrDefaultAsync();
        }

    }
}
