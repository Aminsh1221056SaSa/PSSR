
using Microsoft.EntityFrameworkCore;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ActivityServices;
using PSSR.ServiceLayer.ProjectServices;
using PSSR.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.PlanServices.Concrete
{
    public class ListPlanService
    {
        private readonly EfCoreContext _context;

        public ListPlanService(EfCoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FormDicPlanModelDto>> GetFormDictionaryGroupedByPlanDate(int workId, int locationId, 
            int subSystemId, int desId)
        {
            var FormDics = await _context.FormDictionaries.Where(s => s.Activityes.Any(o => o.SubsytemId == subSystemId && o.LocationId==locationId
            && o.WorkPackageId==workId && o.DesciplineId==desId))
               .ToDictionaryAsync(x => x.Id, x => new Tuple<string, string>(x.Code, x.Description));

            return await getActivityFormDicPlane(workId,locationId,subSystemId,desId, FormDics);
        }

        public async Task<IEnumerable<DesciplinePlanModelDto>> getActivityDescplineGroupedByWorkPackage(int workPackageId, 
            int locationId, int systemId,
            long subsystemId)
        {
            return await this.getSubSystemPlane(workPackageId,locationId,systemId,subsystemId);
        }

        public async Task<IEnumerable<HirecharyPlaneDto>> getPlanHirechary(string filterTypes,Guid projectId)
        {
            var model = new TaskGroupModel();
            var sType = filterTypes.Split(',');
            if(sType.Contains("1000"))
            {
                model.GroupByWorkPackage = true;
            }
            if (sType.Contains("1001"))
            {
                model.GroupByLocation = true;
            }
            if (sType.Contains("1002"))
            {
                model.GroupByDescipline = true;
            }
            if (sType.Contains("1003"))
            {
                model.GroupBySystem = true;
            }
            if (sType.Contains("1004"))
            {
                model.GroupBySubSystem = true;
            }

            var parentItems = await this.getHirecharPlan(model,projectId);

            var FormDics = await _context.FormDictionaries
                .ToDictionaryAsync(x => x.Id, x => new Tuple<string, string>(x.Code, x.Description));


            var systemIds = await _context.ProjectSystems.Where(s => s.ProjectId == projectId)
                .SelectMany(s => s.ProjectSubSystems).Select(s => s.Id).ToArrayAsync();

            var activityes = await _context.Activites.Where(s =>s.PlanEndDate.HasValue && s.PlanStartDate.HasValue
            && systemIds.Contains(s.SubsytemId)).Select(s=>new ActivityListPlanDto
            {
                Id=s.Id,
                LocationId=s.LocationId,
                DesciplineId=s.DesciplineId,
                PlanStartDate=s.PlanStartDate.Value,
                PlanEndDate=s.PlanEndDate.Value,
                FormDictionaryId=s.FormDictionaryId,
                ActivityCode=s.ActivityCode,
                Status=s.Status,
                SubsytemId=s.SubsytemId,
                SystemdId=s.SubSystem.ProjectSystemId,
                TagNumber=s.TagNumber,
                ValueUnitId=s.ValueUnitId,
                WeightFactor=s.WeightFactor,
                WorkPackageId=s.WorkPackageId,
                WorkPackageStepId=s.WorkPackageStepId
            }).ToListAsync();


            foreach (var parent in parentItems)
            {
                var lstChilds = new List<HirecharyPlaneDto>();

                this.getLastChild(parent,lstChilds);
                foreach(var child in lstChilds)
                {
                    var childs = this.getActivityHirecharyPlane(child.WorkPackageId, child.LocationId, child.DesciplineId, child.SystemId, child.SubSystemId, child.Id, FormDics, model, projectId, activityes);

                    child.Childeren.AddRange(childs);
                }
            }
            return parentItems.OrderBy(s=>s.Id).ThenBy(s=>s.Childeren.Min(o=>o.StartDate));
        }
        
        //private methods
        private async Task<IEnumerable<HirecharyPlaneDto>> getHirecharPlan(TaskGroupModel model,Guid projectId)
        {
            var allActivity = await _context.Activites.Where(s =>s.PlanStartDate.HasValue && s.PlanEndDate.HasValue)
                .Select(x=>new
            {
                x.WorkPackageId,
                x.LocationId,
                x.DesciplineId,
                x.WorkPackageStepId,
                x.SubsytemId,
                x.SubSystem.ProjectSystemId
            }).ToListAsync();

            var lstItems = new List<HirecharyPlaneDto>();

            if(model.GroupByWorkPackage)
            {
                var workpackages = await _context.ProjectRoadMaps.OrderBy(s => s.Id)
                    .ToDictionaryAsync(x => x.Id, s => s.Name);

                Parallel.ForEach(allActivity.GroupBy(s => new { s.WorkPackageId }), acg =>
                {
                    var item = new HirecharyPlaneDto
                    {
                        Title = workpackages[acg.Key.WorkPackageId],
                        Id = acg.Key.WorkPackageId,
                        ParentId = null,
                        WorkPackageId = acg.Key.WorkPackageId
                    };
                    lstItems.Add(item);
                });
            }
            
            if(model.GroupByLocation)
            {
                var locations = await _context.LocationTypes.OrderBy(s => s.Id)
                  .ToDictionaryAsync(x => x.Id, s => s.Title);
                if (model.GroupByWorkPackage)
                {
                    Parallel.ForEach(allActivity.GroupBy(s => new { s.WorkPackageId, s.LocationId }), loc =>
                    {
                        var item = lstItems.FirstOrDefault(s => s.WorkPackageId == loc.Key.WorkPackageId);
                        if(item!=null)
                        {
                            var locationChild = new HirecharyPlaneDto
                            {
                                Title = locations[loc.Key.LocationId],
                                Id = int.Parse($"{loc.Key.LocationId}{loc.Key.WorkPackageId}"),
                                ParentId = item.Id,
                                LocationId = loc.Key.LocationId,
                                WorkPackageId = loc.Key.WorkPackageId
                            };
                            item.Childeren.Add(locationChild);
                        }
                     });
                }
                else
                {
                    Parallel.ForEach(allActivity.GroupBy(s => new { s.LocationId }), acg =>
                    {
                        var item = new HirecharyPlaneDto
                        {
                            Title = locations[acg.Key.LocationId],
                            Id = acg.Key.LocationId,
                            ParentId = null,
                            LocationId = acg.Key.LocationId,
                            WorkPackageId = null
                        };
                        lstItems.Add(item);
                    });
                }
            }
            
            if(model.GroupBySystem)
            {
                var systems = await _context.ProjectSystems.Where(s=>s.ProjectId==projectId).OrderBy(s => s.Id)
                    .ToDictionaryAsync(x => x.Id, s => s.Code);

                if (model.GroupByWorkPackage)
                {
                    if(model.GroupByLocation)
                    {
                        Parallel.ForEach(allActivity
                            .GroupBy(s => new { s.WorkPackageId, s.LocationId,s.ProjectSystemId }), des =>
                        {
                            var item = lstItems.SelectMany(s => s.Childeren).FirstOrDefault(s =>s.WorkPackageId==des.Key.WorkPackageId &&
                            s.LocationId == des.Key.LocationId);

                            if(item!=null)
                            {
                                var locationChild = new HirecharyPlaneDto
                                {
                                    Title = systems[des.Key.ProjectSystemId],
                                    Id = int.Parse($"{des.Key.ProjectSystemId}{des.Key.LocationId}{des.Key.WorkPackageId}"),
                                    ParentId = item.Id,
                                    SystemId = des.Key.ProjectSystemId,
                                    WorkPackageId = des.Key.WorkPackageId,
                                    LocationId = des.Key.LocationId,
                                    StartDate = DateTime.Now.ToString("d"),
                                    EndDate = DateTime.Now.ToString("d")
                                };
                                item.Childeren.Add(locationChild);
                            }
                           
                        });
                    }
                    else
                    {
                        Parallel.ForEach(allActivity
                            .GroupBy(s => new { s.WorkPackageId,s.ProjectSystemId }), des =>
                            {
                                var item = lstItems.First(s => s.WorkPackageId == des.Key.WorkPackageId);
                                var locationChild = new HirecharyPlaneDto
                                {
                                    Title = systems[des.Key.ProjectSystemId],
                                    Id = int.Parse($"{des.Key.ProjectSystemId}{des.Key.WorkPackageId}"),
                                    ParentId = item.Id,
                                    SystemId = des.Key.ProjectSystemId,
                                    WorkPackageId = des.Key.WorkPackageId,
                                    LocationId = null,
                                    StartDate = DateTime.Now.ToString("d"),
                                    EndDate = DateTime.Now.ToString("d")
                                };
                                item.Childeren.Add(locationChild);
                            });
                    }
                }
                else
                {
                    if (model.GroupByLocation)
                    {
                        Parallel.ForEach(allActivity
                            .GroupBy(s => new {s.LocationId, s.ProjectSystemId }), des =>
                            {
                                var item = lstItems.FirstOrDefault(s => s.LocationId == des.Key.LocationId);
                                if(item!=null)
                                {
                                    var locationChild = new HirecharyPlaneDto
                                    {
                                        Title = systems[des.Key.ProjectSystemId],
                                        Id = int.Parse($"{des.Key.ProjectSystemId}{des.Key.LocationId}"),
                                        ParentId = item.Id,
                                        SystemId = des.Key.ProjectSystemId,
                                        WorkPackageId = null,
                                        LocationId = des.Key.LocationId,
                                        StartDate = DateTime.Now.ToString("d"),
                                        EndDate = DateTime.Now.ToString("d")
                                    };
                                    item.Childeren.Add(locationChild);
                                }
                            });
                    }
                    else
                    {
                        Parallel.ForEach(allActivity
                            .GroupBy(s => new { s.ProjectSystemId }), acg =>
                        {
                            var item = new HirecharyPlaneDto
                            {
                                Title = systems[acg.Key.ProjectSystemId],
                                Id = acg.Key.ProjectSystemId,
                                ParentId = null,
                                SystemId = acg.Key.ProjectSystemId,
                                StartDate = DateTime.Now.ToString("d"),
                                EndDate = DateTime.Now.ToString("d")
                            };
                            lstItems.Add(item);
                        });
                    }
                }
            }

            if(model.GroupBySubSystem)
            {
                var subSystems = await _context.ProjectSubSystems.Where(s=>s.ProjectSystem.ProjectId==projectId).OrderBy(s => s.Id)
                  .ToDictionaryAsync(x => x.Id, s => s.Code);

                if (model.GroupByWorkPackage)
                {
                    if(model.GroupByLocation)
                    {
                        if(model.GroupBySystem)
                        {
                            Parallel.ForEach(allActivity
                            .GroupBy(s => new {s.ProjectSystemId, s.WorkPackageId, s.LocationId, s.SubsytemId }), des =>
                            {
                                var item = lstItems.SelectMany(s => s.Childeren).SelectMany(s=>s.Childeren)
                                .FirstOrDefault(s => s.WorkPackageId == des.Key.WorkPackageId &&
                                s.LocationId == des.Key.LocationId && s.SystemId.Value==des.Key.ProjectSystemId);

                                if(item!=null)
                                {
                                    var locationChild = new HirecharyPlaneDto
                                    {
                                        Title = subSystems[des.Key.SubsytemId],
                                        Id = int.Parse($"{des.Key.SubsytemId}{des.Key.ProjectSystemId}{des.Key.LocationId}{des.Key.WorkPackageId}"),
                                        ParentId = item.Id,
                                        DesciplineId = null,
                                        WorkPackageId = des.Key.WorkPackageId,
                                        LocationId = des.Key.LocationId,
                                        SystemId = des.Key.ProjectSystemId,
                                        SubSystemId = des.Key.SubsytemId,
                                        StartDate = DateTime.Now.ToString("d"),
                                        EndDate = DateTime.Now.ToString("d")
                                    };
                                    item.Childeren.Add(locationChild);
                                }
                               
                            });
                        }
                        else
                        {
                            Parallel.ForEach(allActivity
                           .GroupBy(s => new { s.SubsytemId, s.WorkPackageId, s.LocationId}), des =>
                           {
                               var item = lstItems.SelectMany(s => s.Childeren)
                               .FirstOrDefault(s => s.WorkPackageId == des.Key.WorkPackageId &&
                               s.LocationId == des.Key.LocationId);

                               if(item!=null)
                               {
                                   var locationChild = new HirecharyPlaneDto
                                   {
                                       Title = subSystems[des.Key.SubsytemId],
                                       Id = int.Parse($"{des.Key.SubsytemId}{des.Key.LocationId}{des.Key.WorkPackageId}"),
                                       ParentId = item.Id,
                                       DesciplineId = null,
                                       WorkPackageId = des.Key.WorkPackageId,
                                       LocationId = des.Key.LocationId,
                                       SystemId = null,
                                       SubSystemId = des.Key.SubsytemId,
                                       StartDate = DateTime.Now.ToString("d"),
                                       EndDate = DateTime.Now.ToString("d")
                                   };
                                   item.Childeren.Add(locationChild);
                               }
                               
                           });
                        }
                    }
                    else
                    {
                        if (model.GroupBySystem)
                        {
                            Parallel.ForEach(allActivity
                           .GroupBy(s => new { s.SubsytemId, s.WorkPackageId, s.ProjectSystemId }), des =>
                           {
                               var item = lstItems.SelectMany(s => s.Childeren)
                               .FirstOrDefault(s => s.WorkPackageId == des.Key.WorkPackageId &&
                               s.SystemId == des.Key.ProjectSystemId);

                               if(item!=null)
                               {
                                   var locationChild = new HirecharyPlaneDto
                                   {
                                       Title = subSystems[des.Key.SubsytemId],
                                       Id = int.Parse($"{des.Key.SubsytemId}{des.Key.ProjectSystemId}{des.Key.WorkPackageId}"),
                                       ParentId = item.Id,
                                       DesciplineId = null,
                                       WorkPackageId = des.Key.WorkPackageId,
                                       LocationId = null,
                                       SystemId = des.Key.ProjectSystemId,
                                       SubSystemId = des.Key.SubsytemId,
                                       StartDate = DateTime.Now.ToString("d"),
                                       EndDate = DateTime.Now.ToString("d")
                                   };
                                   item.Childeren.Add(locationChild);
                               }
                               
                           });
                        }
                        else
                        {
                            Parallel.ForEach(allActivity
                         .GroupBy(s => new { s.SubsytemId, s.WorkPackageId }), des =>
                         {
                             var item = lstItems.FirstOrDefault(s => s.WorkPackageId == des.Key.WorkPackageId);

                             if(item!=null)
                             {
                                 var locationChild = new HirecharyPlaneDto
                                 {
                                     Title = subSystems[des.Key.SubsytemId],
                                     Id = int.Parse($"{des.Key.SubsytemId}{des.Key.WorkPackageId}"),
                                     ParentId = item.Id,
                                     DesciplineId = null,
                                     WorkPackageId = des.Key.WorkPackageId,
                                     LocationId = null,
                                     SystemId = null,
                                     SubSystemId = des.Key.SubsytemId,
                                     StartDate = DateTime.Now.ToString("d"),
                                     EndDate = DateTime.Now.ToString("d")
                                 };
                                 item.Childeren.Add(locationChild);
                             }
                           
                         });
                        }
                    }
                }
                else
                {
                    if (model.GroupByLocation)
                    {
                        if (model.GroupBySystem)
                        {
                            Parallel.ForEach(allActivity
                          .GroupBy(s => new { s.SubsytemId, s.LocationId, s.ProjectSystemId }), des =>
                          {
                              var item = lstItems.SelectMany(s => s.Childeren)
                              .FirstOrDefault(s => s.SystemId == des.Key.ProjectSystemId &&
                              s.LocationId == des.Key.LocationId);

                              if(item!=null)
                              {
                                  var locationChild = new HirecharyPlaneDto
                                  {
                                      Title = subSystems[des.Key.ProjectSystemId],
                                      Id = int.Parse($"{des.Key.SubsytemId}{des.Key.ProjectSystemId}{des.Key.LocationId}"),
                                      ParentId = item.Id,
                                      SubSystemId = des.Key.SubsytemId,
                                      WorkPackageId = null,
                                      LocationId = des.Key.LocationId,
                                      SystemId = des.Key.ProjectSystemId,
                                      DesciplineId = null,
                                      StartDate = DateTime.Now.ToString("d"),
                                      EndDate = DateTime.Now.ToString("d")
                                  };
                                  item.Childeren.Add(locationChild);
                              }
                             
                          });

                        }
                        else
                        {
                            Parallel.ForEach(allActivity
                          .GroupBy(s => new { s.SubsytemId, s.LocationId }), des =>
                          {
                              var item = lstItems.FirstOrDefault(s => s.LocationId == des.Key.LocationId);

                              if(item!=null)
                              {
                                  var locationChild = new HirecharyPlaneDto
                                  {
                                      Title = subSystems[des.Key.SubsytemId],
                                      Id = int.Parse($"{des.Key.SubsytemId}{des.Key.LocationId}"),
                                      ParentId = item.Id,
                                      DesciplineId = null,
                                      WorkPackageId = null,
                                      SystemId = null,
                                      LocationId = des.Key.LocationId,
                                      SubSystemId = des.Key.SubsytemId,
                                      StartDate = DateTime.Now.ToString("d"),
                                      EndDate = DateTime.Now.ToString("d")
                                  };
                                  item.Childeren.Add(locationChild);
                              }
                             
                          });
                        }
                    }
                    else
                    {
                        if (model.GroupBySystem)
                        {
                            Parallel.ForEach(allActivity
                         .GroupBy(s => new { s.SubsytemId, s.ProjectSystemId }), des =>
                         {
                             var item = lstItems.FirstOrDefault(s => s.SystemId == des.Key.ProjectSystemId);

                             if(item!=null)
                             {
                                 var locationChild = new HirecharyPlaneDto
                                 {
                                     Title = subSystems[des.Key.SubsytemId],
                                     Id = int.Parse($"{des.Key.SubsytemId}{des.Key.ProjectSystemId}"),
                                     ParentId = item.Id,
                                     DesciplineId = null,
                                     WorkPackageId = null,
                                     LocationId = null,
                                     SystemId = des.Key.ProjectSystemId,
                                     SubSystemId=des.Key.SubsytemId,
                                     StartDate = DateTime.Now.ToString("d"),
                                     EndDate = DateTime.Now.ToString("d")
                                 };
                                 item.Childeren.Add(locationChild);
                             }
                         });
                        }
                        else
                        {
                            Parallel.ForEach(allActivity
                            .GroupBy(s => new { s.SubsytemId }), acg =>
                            {
                                var item = new HirecharyPlaneDto
                                {
                                    Title = subSystems[acg.Key.SubsytemId],
                                    Id = acg.Key.SubsytemId,
                                    ParentId = null,
                                    DesciplineId = null,
                                    SystemId=null,
                                    SubSystemId=acg.Key.SubsytemId,
                                    StartDate = DateTime.Now.ToString("d"),
                                    EndDate = DateTime.Now.ToString("d")
                                };
                                lstItems.Add(item);
                            });
                        }
                    }
                }
            }

            if(model.GroupByDescipline)
            {
                var desciplines = await _context.Desciplines.OrderBy(s => s.Id)
                  .ToDictionaryAsync(x => x.Id, s => s.Name);

                if (model.GroupByWorkPackage)
                {
                    if(model.GroupByLocation)
                    {
                        if(model.GroupBySystem)
                        {
                            if (model.GroupBySubSystem)
                            {
                                Parallel.ForEach(allActivity
                           .GroupBy(s => new { s.SubsytemId ,s.ProjectSystemId, s.WorkPackageId, s.LocationId, s.DesciplineId}), des =>
                            {
                                var item = lstItems.SelectMany(s => s.Childeren).SelectMany(s => s.Childeren)
                                .SelectMany(s => s.Childeren)
                                .FirstOrDefault(s => s.WorkPackageId == des.Key.WorkPackageId &&
                                s.LocationId == des.Key.LocationId && s.SubSystemId.Value == des.Key.SubsytemId
                                && s.SystemId == des.Key.ProjectSystemId);

                                if(item!=null)
                                {
                                    var locationChild = new HirecharyPlaneDto
                                    {
                                        Title = desciplines[des.Key.DesciplineId],
                                        Id = int.Parse($"{des.Key.DesciplineId}{des.Key.SubsytemId}{des.Key.ProjectSystemId}{des.Key.LocationId}{des.Key.WorkPackageId}"),
                                        ParentId = item.Id,
                                        DesciplineId = des.Key.DesciplineId,
                                        WorkPackageId = des.Key.WorkPackageId,
                                        LocationId = des.Key.LocationId,
                                        SystemId = des.Key.ProjectSystemId,
                                        SubSystemId = des.Key.SubsytemId,
                                        StartDate = DateTime.Now.ToString("d"),
                                        EndDate = DateTime.Now.ToString("d")
                                    };
                                    item.Childeren.Add(locationChild);
                                }
                            });
                            }
                            else
                            {
                                Parallel.ForEach(allActivity
                           .GroupBy(s => new {s.WorkPackageId, s.LocationId, s.ProjectSystemId,s.DesciplineId }), des =>
                            {
                                var item = lstItems.SelectMany(s => s.Childeren)
                                .SelectMany(s => s.Childeren)
                                .FirstOrDefault(s => s.WorkPackageId == des.Key.WorkPackageId &&
                                s.LocationId == des.Key.LocationId && s.SystemId.Value == des.Key.ProjectSystemId);

                                if(item!=null)
                                {
                                    var locationChild = new HirecharyPlaneDto
                                    {
                                        Title = desciplines[des.Key.DesciplineId],
                                        Id = int.Parse($"{des.Key.DesciplineId}{des.Key.ProjectSystemId}{des.Key.LocationId}{des.Key.WorkPackageId}"),
                                        ParentId = item.Id,
                                        DesciplineId = des.Key.DesciplineId,
                                        WorkPackageId = des.Key.WorkPackageId,
                                        LocationId = des.Key.LocationId,
                                        SystemId = des.Key.ProjectSystemId,
                                        SubSystemId = null,
                                        StartDate = DateTime.Now.ToString("d"),
                                        EndDate = DateTime.Now.ToString("d")
                                    };
                                    item.Childeren.Add(locationChild);
                                }
                            });
                            }
                        }
                        else
                        {
                            if(model.GroupBySubSystem)
                            {
                                Parallel.ForEach(allActivity
                           .GroupBy(s => new { s.SubsytemId, s.WorkPackageId, s.LocationId, s.DesciplineId }), des =>
                           {
                               var item = lstItems.SelectMany(s => s.Childeren)
                               .SelectMany(s => s.Childeren)
                               .FirstOrDefault(s => s.WorkPackageId == des.Key.WorkPackageId &&
                               s.LocationId == des.Key.LocationId && s.SubSystemId.Value == des.Key.SubsytemId);

                               if (item != null)
                               {
                                   var locationChild = new HirecharyPlaneDto
                                   {
                                       Title = desciplines[des.Key.DesciplineId],
                                       Id = int.Parse($"{des.Key.DesciplineId}{des.Key.SubsytemId}{des.Key.LocationId}{des.Key.WorkPackageId}"),
                                       ParentId = item.Id,
                                       DesciplineId = des.Key.DesciplineId,
                                       WorkPackageId = des.Key.WorkPackageId,
                                       LocationId = des.Key.LocationId,
                                       SystemId = null,
                                       SubSystemId = des.Key.SubsytemId,
                                       StartDate = DateTime.Now.ToString("d"),
                                       EndDate = DateTime.Now.ToString("d")
                                   };
                                   item.Childeren.Add(locationChild);
                               }
                           });
                            }
                            else
                            {
                                Parallel.ForEach(allActivity
                          .GroupBy(s => new { s.DesciplineId, s.WorkPackageId, s.LocationId}), des =>
                          {
                              var item = lstItems.SelectMany(s => s.Childeren)
                              .FirstOrDefault(s => s.WorkPackageId == des.Key.WorkPackageId &&
                              s.LocationId == des.Key.LocationId);

                              if (item != null)
                              {
                                  var locationChild = new HirecharyPlaneDto
                                  {
                                      Title = desciplines[des.Key.DesciplineId],
                                      Id = int.Parse($"{des.Key.DesciplineId}{des.Key.LocationId}{des.Key.WorkPackageId}"),
                                      ParentId = item.Id,
                                      DesciplineId = des.Key.DesciplineId,
                                      WorkPackageId = des.Key.WorkPackageId,
                                      LocationId = des.Key.LocationId,
                                      SystemId = null,
                                      SubSystemId = null,
                                      StartDate = DateTime.Now.ToString("d"),
                                      EndDate = DateTime.Now.ToString("d")
                                  };
                                  item.Childeren.Add(locationChild);
                              }
                          });
                            }
                        }
                    }
                    else
                    {
                        if(model.GroupBySystem)
                        {
                            if(model.GroupBySubSystem)
                            {
                                Parallel.ForEach(allActivity
                         .GroupBy(s => new {s.SubsytemId, s.ProjectSystemId, s.WorkPackageId, s.DesciplineId }), des =>
                         {
                             var item = lstItems.SelectMany(s => s.Childeren).SelectMany(s => s.Childeren)
                             .FirstOrDefault(s => s.WorkPackageId == des.Key.WorkPackageId &&
                             s.SystemId == des.Key.ProjectSystemId && s.SubSystemId==des.Key.SubsytemId);

                             if (item != null)
                             {
                                 var locationChild = new HirecharyPlaneDto
                                 {
                                     Title = desciplines[des.Key.DesciplineId],
                                     Id = int.Parse($"{des.Key.DesciplineId}{des.Key.SubsytemId}{des.Key.ProjectSystemId}{des.Key.WorkPackageId}"),
                                     ParentId = item.Id,
                                     DesciplineId = des.Key.DesciplineId,
                                     WorkPackageId = des.Key.WorkPackageId,
                                     LocationId = null,
                                     SystemId = des.Key.ProjectSystemId,
                                     SubSystemId = des.Key.SubsytemId,
                                     StartDate = DateTime.Now.ToString("d"),
                                     EndDate = DateTime.Now.ToString("d")
                                 };
                                 item.Childeren.Add(locationChild);
                             }
                         });
                            }
                            else
                            {
                                Parallel.ForEach(allActivity
                         .GroupBy(s => new { s.ProjectSystemId, s.WorkPackageId, s.DesciplineId }), des =>
                         {
                             var item = lstItems.SelectMany(s => s.Childeren)
                             .FirstOrDefault(s => s.WorkPackageId == des.Key.WorkPackageId &&
                             s.SystemId == des.Key.ProjectSystemId);

                             if (item != null)
                             {
                                 var locationChild = new HirecharyPlaneDto
                                 {
                                     Title = desciplines[des.Key.DesciplineId],
                                     Id = int.Parse($"{des.Key.DesciplineId}{des.Key.ProjectSystemId}{des.Key.WorkPackageId}"),
                                     ParentId = item.Id,
                                     DesciplineId = des.Key.DesciplineId,
                                     WorkPackageId = des.Key.WorkPackageId,
                                     LocationId = null,
                                     SystemId = des.Key.ProjectSystemId,
                                     SubSystemId = null,
                                     StartDate = DateTime.Now.ToString("d"),
                                     EndDate = DateTime.Now.ToString("d")
                                 };
                                 item.Childeren.Add(locationChild);
                             }
                         });
                            }
                           
                        }
                        else
                        {
                            if(model.GroupBySubSystem)
                            {
                                Parallel.ForEach(allActivity
                          .GroupBy(s => new { s.DesciplineId, s.WorkPackageId, s.SubsytemId }), des =>
                          {
                              var item = lstItems.SelectMany(s => s.Childeren)
                              .FirstOrDefault(s => s.WorkPackageId == des.Key.WorkPackageId &&
                              s.SubSystemId == des.Key.SubsytemId);

                              if (item != null)
                              {
                                  var locationChild = new HirecharyPlaneDto
                                  {
                                      Title = desciplines[des.Key.DesciplineId],
                                      Id = int.Parse($"{des.Key.DesciplineId}{des.Key.SubsytemId}{des.Key.WorkPackageId}"),
                                      ParentId = item.Id,
                                      DesciplineId = des.Key.DesciplineId,
                                      WorkPackageId = des.Key.WorkPackageId,
                                      LocationId = null,
                                      SystemId = null,
                                      SubSystemId = des.Key.SubsytemId,
                                      StartDate = DateTime.Now.ToString("d"),
                                      EndDate = DateTime.Now.ToString("d")
                                  };
                                  item.Childeren.Add(locationChild);
                              }
                          });
                            }
                            else
                            {
                                Parallel.ForEach(allActivity
                          .GroupBy(s => new { s.DesciplineId, s.WorkPackageId}), des =>
                          {
                              var item = lstItems
                              .FirstOrDefault(s => s.WorkPackageId == des.Key.WorkPackageId );

                              if (item != null)
                              {
                                  var locationChild = new HirecharyPlaneDto
                                  {
                                      Title = desciplines[des.Key.DesciplineId],
                                      Id = int.Parse($"{des.Key.DesciplineId}{des.Key.WorkPackageId}"),
                                      ParentId = item.Id,
                                      DesciplineId = des.Key.DesciplineId,
                                      WorkPackageId = des.Key.WorkPackageId,
                                      LocationId = null,
                                      SystemId = null,
                                      SubSystemId = null,
                                      StartDate = DateTime.Now.ToString("d"),
                                      EndDate = DateTime.Now.ToString("d")
                                  };
                                  item.Childeren.Add(locationChild);
                              }
                          });
                            }
                        }
                    }
                }
                else
                {
                   if(model.GroupByLocation)
                    {
                        if (model.GroupBySystem)
                        {
                            if (model.GroupBySubSystem)
                            {
                                Parallel.ForEach(allActivity
                          .GroupBy(s => new { s.DesciplineId, s.ProjectSystemId, s.SubsytemId,s.LocationId }), des =>
                          {
                              var item = lstItems.SelectMany(s => s.Childeren).SelectMany(s => s.Childeren)
                              .FirstOrDefault(s => s.SystemId == des.Key.ProjectSystemId &&
                              s.SubSystemId == des.Key.SubsytemId && s.LocationId==des.Key.LocationId);

                              if (item != null)
                              {
                                  var locationChild = new HirecharyPlaneDto
                                  {
                                      Title = desciplines[des.Key.DesciplineId],
                                      Id = int.Parse($"{des.Key.DesciplineId}{des.Key.SubsytemId}{des.Key.ProjectSystemId}{des.Key.LocationId}"),
                                      ParentId = item.Id,
                                      DesciplineId = des.Key.DesciplineId,
                                      WorkPackageId = null,
                                      LocationId = des.Key.LocationId,
                                      SystemId = des.Key.ProjectSystemId,
                                      SubSystemId = des.Key.SubsytemId,
                                      StartDate = DateTime.Now.ToString("d"),
                                      EndDate = DateTime.Now.ToString("d")
                                  };
                                  item.Childeren.Add(locationChild);
                              }
                          });
                            }
                            else
                            {
                                Parallel.ForEach(allActivity
                          .GroupBy(s => new { s.DesciplineId, s.ProjectSystemId,s.LocationId }), des =>
                          {
                              var item = lstItems.SelectMany(s=>s.Childeren)
                              .FirstOrDefault(s => s.SystemId == des.Key.ProjectSystemId && s.LocationId==des.Key.LocationId);

                              if (item != null)
                              {
                                  var locationChild = new HirecharyPlaneDto
                                  {
                                      Title = desciplines[des.Key.DesciplineId],
                                      Id = int.Parse($"{des.Key.DesciplineId}{des.Key.ProjectSystemId}{des.Key.LocationId}"),
                                      ParentId = item.Id,
                                      DesciplineId = des.Key.DesciplineId,
                                      WorkPackageId = null,
                                      LocationId = des.Key.LocationId,
                                      SystemId = des.Key.ProjectSystemId,
                                      SubSystemId = null,
                                      StartDate = DateTime.Now.ToString("d"),
                                      EndDate = DateTime.Now.ToString("d")
                                  };
                                  item.Childeren.Add(locationChild);
                              }
                          });
                            }
                        }
                        else
                        {
                            if (model.GroupBySubSystem)
                            {
                                Parallel.ForEach(allActivity
                          .GroupBy(s => new { s.DesciplineId, s.SubsytemId,s.LocationId }), des =>
                          {
                              var item = lstItems.SelectMany(s=>s.Childeren)
                              .FirstOrDefault(s => s.SubSystemId == des.Key.SubsytemId && s.LocationId==des.Key.LocationId);

                              if (item != null)
                              {
                                  var locationChild = new HirecharyPlaneDto
                                  {
                                      Title = desciplines[des.Key.DesciplineId],
                                      Id = int.Parse($"{des.Key.DesciplineId}{des.Key.SubsytemId}{des.Key.LocationId}"),
                                      ParentId = item.Id,
                                      DesciplineId = des.Key.DesciplineId,
                                      WorkPackageId = null,
                                      LocationId = des.Key.LocationId,
                                      SystemId = null,
                                      SubSystemId = des.Key.SubsytemId,
                                      StartDate = DateTime.Now.ToString("d"),
                                      EndDate = DateTime.Now.ToString("d")
                                  };
                                  item.Childeren.Add(locationChild);
                              }
                          });
                            }
                            else
                            {
                                Parallel.ForEach(allActivity
                          .GroupBy(s => new { s.DesciplineId,s.LocationId }), des =>
                          {
                              var item = lstItems
                              .FirstOrDefault(s => s.LocationId == des.Key.LocationId);

                              if (item != null)
                              {
                                  var locationChild = new HirecharyPlaneDto
                                  {
                                      Title = desciplines[des.Key.DesciplineId],
                                      Id = int.Parse($"{des.Key.DesciplineId}{des.Key.LocationId}"),
                                      ParentId = item.Id,
                                      DesciplineId = des.Key.DesciplineId,
                                      WorkPackageId = null,
                                      LocationId = des.Key.LocationId,
                                      SystemId =null,
                                      SubSystemId =null,
                                      StartDate = DateTime.Now.ToString("d"),
                                      EndDate = DateTime.Now.ToString("d")
                                  };
                                  item.Childeren.Add(locationChild);
                              }
                          });
                            }
                        }
                    }
                   else
                    {
                        if (model.GroupBySystem)
                        {
                            if(model.GroupBySubSystem)
                            {
                                Parallel.ForEach(allActivity
                          .GroupBy(s => new { s.SubsytemId, s.ProjectSystemId, s.DesciplineId }), des =>
                          {
                              var item = lstItems.SelectMany(s => s.Childeren)
                              .FirstOrDefault(s => s.SystemId == des.Key.ProjectSystemId &&
                              s.SubSystemId == des.Key.SubsytemId);

                              if (item != null)
                              {
                                  var locationChild = new HirecharyPlaneDto
                                  {
                                      Title = desciplines[des.Key.DesciplineId],
                                      Id = int.Parse($"{des.Key.DesciplineId}{des.Key.SubsytemId}{des.Key.ProjectSystemId}"),
                                      ParentId = item.Id,
                                      DesciplineId = des.Key.DesciplineId,
                                      WorkPackageId = null,
                                      LocationId = null,
                                      SystemId = des.Key.ProjectSystemId,
                                      SubSystemId = des.Key.SubsytemId,
                                      StartDate = DateTime.Now.ToString("d"),
                                      EndDate = DateTime.Now.ToString("d")
                                  };
                                  item.Childeren.Add(locationChild);
                              }
                          });
                            }
                            else
                            {
                                Parallel.ForEach(allActivity
                          .GroupBy(s => new { s.ProjectSystemId, s.DesciplineId }), des =>
                          {
                              var item = lstItems
                              .FirstOrDefault(s => s.SystemId == des.Key.ProjectSystemId);

                              if (item != null)
                              {
                                  var locationChild = new HirecharyPlaneDto
                                  {
                                      Title = desciplines[des.Key.DesciplineId],
                                      Id = int.Parse($"{des.Key.DesciplineId}{des.Key.ProjectSystemId}"),
                                      ParentId = item.Id,
                                      DesciplineId = des.Key.DesciplineId,
                                      WorkPackageId = null,
                                      LocationId = null,
                                      SystemId = des.Key.ProjectSystemId,
                                      SubSystemId = null,
                                      StartDate = DateTime.Now.ToString("d"),
                                      EndDate = DateTime.Now.ToString("d")
                                  };
                                  item.Childeren.Add(locationChild);
                              }
                          });
                            }
                        }
                        else
                        {
                            if (model.GroupBySubSystem)
                            {
                                Parallel.ForEach(allActivity
                          .GroupBy(s => new { s.SubsytemId, s.DesciplineId }), des =>
                          {
                              var item = lstItems
                              .FirstOrDefault(s => s.SubSystemId == des.Key.SubsytemId);

                              if (item != null)
                              {
                                  var locationChild = new HirecharyPlaneDto
                                  {
                                      Title = desciplines[des.Key.DesciplineId],
                                      Id = int.Parse($"{des.Key.DesciplineId}{des.Key.SubsytemId}"),
                                      ParentId = item.Id,
                                      DesciplineId = des.Key.DesciplineId,
                                      WorkPackageId =null,
                                      LocationId = null,
                                      SystemId = null,
                                      SubSystemId = des.Key.SubsytemId,
                                      StartDate = DateTime.Now.ToString("d"),
                                      EndDate = DateTime.Now.ToString("d")
                                  };
                                  item.Childeren.Add(locationChild);
                              }
                          });
                            }
                            else
                            {
                                Parallel.ForEach(allActivity
                          .GroupBy(s => new { s.DesciplineId }), acg =>
                          {
                              var item = new HirecharyPlaneDto
                              {
                                  Title = desciplines[acg.Key.DesciplineId],
                                  Id = acg.Key.DesciplineId,
                                  ParentId = null,
                                  DesciplineId =acg.Key.DesciplineId ,
                                  SystemId = null,
                                  SubSystemId = null,
                                  StartDate = DateTime.Now.ToString("d"),
                                  EndDate = DateTime.Now.ToString("d")
                              };
                              lstItems.Add(item);
                          });
                            }
                        }
                    }
                   
                }
            }

            return lstItems;
        }

        private IEnumerable<HirecharyPlaneDto> getActivityHirecharyPlane(int? workId,int? locationId,int? desciplineId,
           long? systemId,long? subSystemId, long parentId ,Dictionary<long, Tuple<string, string>> forms, TaskGroupModel model,Guid projectId,
           IEnumerable<ActivityListPlanDto> query)
        {
            if(model.GroupByWorkPackage)
            {
                query = query.Where(s => s.WorkPackageId == workId);
            }

            if(model.GroupByLocation)
            {
                query = query.Where(s => s.LocationId == locationId);
            }

            if (model.GroupByDescipline)
            {
                query = query.Where(s => s.DesciplineId == desciplineId);
            }

            if (model.GroupBySystem)
            {
                query = query.Where(s => s.SystemdId == systemId);
            }

            if(model.GroupBySubSystem)
            {
                query = query.Where(s => s.SubsytemId == subSystemId);
            }

            var allActivity = query.GroupBy(s =>new {s.FormDictionaryId}).ToList();

            var lstItems = new List<HirecharyPlaneDto>();

            Parallel.ForEach(allActivity, acg =>
             {
                 var item = new HirecharyPlaneDto
                 {
                     Id = long.Parse($"{parentId}{acg.Key.FormDictionaryId}"),
                     ParentId = parentId
                 };

                 var form = forms.Where(s => s.Key == acg.Key.FormDictionaryId).First();
                 item.Title = form.Value.Item1;
                 item.Resources = form.Value.Item2;

                 item.StartDateActual = acg.Min(s => s.PlanStartDate);

                 item.Total = acg.Count();
                 item.Done = acg.Count(s => s.Status == Common.ActivityStatus.Done);
                 item.StartDate = item.StartDateActual.ToString("d");
                 item.StartTime = acg.Min(s => s.PlanStartDate).ToString("hh:mm tt", CultureInfo.InvariantCulture);
                 item.EndDate = acg.Max(s => s.PlanEndDate).ToString("d");
                 item.EndTime = acg.Max(s => s.PlanEndDate).ToString("hh:mm tt", CultureInfo.InvariantCulture);

                 lstItems.Add(item);
             });

            return lstItems.OrderBy(s => s.StartDateActual);
        }

        private async Task<IEnumerable<DesciplinePlanModelDto>> getSubSystemPlane(int workPackageId,
            int locationId, int systemId,
            long subsystemId)
        {
            var query = _context.Activites.AsQueryable();

            if(workPackageId>0)
            {
                query = query.Where(s => s.WorkPackageId == workPackageId);
            }

            if (locationId > 0)
            {
                query = query.Where(s => s.LocationId == locationId);
            }

            if (systemId > 0)
            {
                query = query.Where(s => s.SubSystem.ProjectSystemId == systemId);
            }

            if (subsystemId > 0)
            {
                query = query.Where(s => s.SubsytemId == subsystemId);
            }

            var allActivity = await query.GroupBy(s => new { s.DesciplineId }).ToListAsync();

            var desciplines = await _context.Desciplines.OrderBy(s => s.Id).ToDictionaryAsync(x => x.Id, s => s.Name);

            var lstItems = new List<DesciplinePlanModelDto>();
            Parallel.ForEach(allActivity, acg =>
            {
                var desc = desciplines[acg.Key.DesciplineId];
                var item = new DesciplinePlanModelDto
                {
                    Title = desc,
                    Id = acg.Key.DesciplineId,
                };

                item.Total = acg.Count();
                item.Done = acg.Count(s => s.Status == Common.ActivityStatus.Done);
                var sDate = acg.OrderBy(s => s.PlanStartDate).First().PlanStartDate;
                var endate = acg.OrderByDescending(s => s.PlanEndDate).First().PlanEndDate;

                item.StartDate = sDate != null ? sDate.Value.ToString("d") : "[N/A]";
                item.EndDate = endate != null ? endate.Value.ToString("d") : "[N/A]";

                lstItems.Add(item);
            });
            return lstItems;
        }

        private async Task<IEnumerable<FormDicPlanModelDto>> getActivityFormDicPlane(int workId, int locationId, int subSystemId, 
            int desId, Dictionary<long, Tuple<string, string>> forms)
        {
            var allActivity = await _context.Activites.Where(s => s.PlanStartDate.HasValue && s.PlanEndDate.HasValue &&
             s.SubsytemId == subSystemId && s.WorkPackageId == workId && s.LocationId==locationId && s.DesciplineId==desId)
               .GroupBy(s => new {s.FormDictionaryId }).ToListAsync();

            var lstItems = new List<FormDicPlanModelDto>();

            foreach (var acg in allActivity)
            {
                var item = new FormDicPlanModelDto
                {
                    Id = acg.Key.FormDictionaryId,
                };

                var form = forms.Where(s => s.Key == acg.Key.FormDictionaryId).First();
                item.Title = form.Value.Item1;
                item.Resources = form.Value.Item2;

                item.Total = acg.Count();
                item.Done = acg.Count(s => s.Status == Common.ActivityStatus.Done);
                item.StartDate = acg.Min(s => s.PlanStartDate).Value.ToString("d");
                item.StartDateActual = acg.Min(s => s.PlanStartDate).Value;
                item.StartTime = acg.Min(s => s.PlanStartDate).Value.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                item.EndDate = acg.Max(s => s.PlanEndDate).Value.ToString("d");
                item.EndTime = acg.Max(s => s.PlanEndDate).Value.ToString("hh:mm tt", CultureInfo.InvariantCulture);

                lstItems.Add(item);
            }

            return lstItems.OrderBy(s => s.StartDateActual.Date);
        }

        private void getLastChild(HirecharyPlaneDto parent,List<HirecharyPlaneDto> childes)
        {
            if (parent.Childeren.Any())
            {
                foreach (var p in parent.Childeren)
                {
                    getLastChild(p,childes);
                }
            }
            else
            {
                childes.Add(parent);
            }
        }
    }
}
