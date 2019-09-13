using PSSR.Common;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.ServiceLayer.ActivityServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.Utils.ProgressHelper
{
    public class ProgressHelper
    {
        internal DataTable CalculateActivityWeithtFactor(List<ProjectWBS> wbsItems, List<ActivityListDetailsDto> allActivity, DataTable dt)
        {
            foreach (var sb in wbsItems)
            {
                IEnumerable < ActivityListDetailsDto > lstItems = this.GetActivityForWbs(sb, allActivity); 

                if (lstItems != null && lstItems.Any())
                {
                    int gCount = lstItems.Count();

                    var gbyvaluUnit = lstItems.GroupBy(s => s.ValueUnitId);
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
            }
            return dt;
        }

        internal void CalculateActivityWFRelatedToWBSLastParent(List<ProjectWBS> wbsItems, List<ActivityListDetailsDto> allActivity,WBSType lastCalcType)
        {
            var destinictLastItems = wbsItems.Where(s => !s.Childeren.Any()
             && s.CalculationType == WfCalculationType.Automatic).ToList();

            foreach(var wb in destinictLastItems)
            {
                if(wb.Parent!=null)
                {
                   wb.LocalWfUpdate(WightFactorCalculationToParentRecovery(wb.WF/100, wb.Parent,lastCalcType));
                }
             
                IEnumerable<ActivityListDetailsDto> lstItems = this.GetActivityForWbs(wb, allActivity);
                foreach(var ac in lstItems)
                {
                    ac.WeightFactor = ((ac.WeightFactor / 100) * wb.WF);
                }
            }
        }

        internal IEnumerable<ProjectWBS> CalculateProgress(List<ProjectWBS> wbsItems, IEnumerable<ActivityListDetailsDto> activityes,bool addActivity)
        {
            var destinictLastItems = wbsItems.Where(s => !s.Childeren.Any()
            && s.CalculationType == WfCalculationType.Automatic).ToList();

            if (!destinictLastItems.Any())
            {
                return wbsItems;
            }

            Parallel.ForEach(destinictLastItems, sb =>
            {
                IEnumerable<ActivityListDetailsDto> lstItems = this.GetActivityForWbs(sb, activityes);

                if (lstItems != null && lstItems.Any())
                {
                    sb.Progress = lstItems.Sum(s => ((s.Progress / 100) * s.WeightFactor));
                    sb.ActivityCount = lstItems.Count();

                    if (addActivity)
                    {
                        foreach (var o in lstItems)
                        {
                            var newItem = ProjectWBS.CreateProjectWBSToParent(WBSType.Activity, o.Id,
                             o.WeightFactor, $"{sb.WBSCode}-({o.ActivityCode})", new Guid(), o.TagNumber, sb, WfCalculationType.Automatic);
                            sb.Childeren.Add(newItem);
                            wbsItems.Add(newItem);
                        }
                    }
                }
                else
                {
                    sb.Progress = 0;
                }
            });

            return wbsItems.OrderBy(s => s.WBSCode);
        }

        internal DataTable CalculateWeightForWBS(List<ProjectWBS> wbsItems,IEnumerable<ActivityListDetailsDto> activityes,DataTable dt)
        {
            foreach(var sb in wbsItems.OrderBy(s=>s.Id))
            {
                var lstItems = this.GetActivityForWbs(sb, activityes);
                if (lstItems != null && lstItems.Any())
                {
                    if (sb.Parent != null)
                    {
                        var parentItems = this.GetActivityForWbs(sb.Parent, activityes);
                        var wf = (lstItems.Sum(s => s.ValueUnitNum) / parentItems.Sum(s => s.ValueUnitNum)) * 100;
                        dt.Rows.Add(sb.Id, wf);
                    }
                }
                else
                {
                    dt.Rows.Add(sb.Id, 0);
                }
            }
            return dt;
        }

        internal ProjectWBS GetClosestParentWeight(ProjectWBS parent)
        {
            if (parent.WF > 0)
            {
                return parent;
            }

            if (parent.Parent != null)
            {
              return  this.GetClosestParentWeight(parent.Parent);
            }
            return null;
        }

        internal float progressRecovery(ProjectWBS parent)
        {
            var snumP = parent.Progress;
            if (parent.Childeren.Any())
            {
                foreach (var p in parent.Childeren)
                {
                    snumP += (this.progressRecovery(p) * (p.WF / 100));
                }
                parent.Progress = snumP;
            }
            return snumP;
        }

        internal void WightFactorRecovery(ProjectWBS parent)
        {
            parent.Progress = parent.WF;
            if (parent.Childeren.Any())
            {
                foreach (var p in parent.Childeren)
                {
                    this.WightFactorRecovery(p);
                }
            }
        }

        internal float WightFactorCalculationToParentRecovery(float cwf,ProjectWBS NextParent,WBSType lastCalcType)
        {
            cwf *= (NextParent.WF / 100);
            if (NextParent.Parent!=null && NextParent.Parent.Type!= lastCalcType)
            {
               cwf= this.WightFactorCalculationToParentRecovery(cwf,NextParent.Parent, lastCalcType);
            }
            return cwf;
        }

        internal int WBSActivityRecovery(ProjectWBS parent)
        {
            var snumP = parent.ActivityCount;
            if (parent.Childeren.Any())
            {
                foreach (var p in parent.Childeren)
                {
                    snumP += this.WBSActivityRecovery(p);
                }
                parent.ActivityCount = snumP;
            }
            return snumP;
        }

        internal void ManualprogressRecovery(IEnumerable<ProjectWBS> wbsItems)
        {
            foreach (var p in wbsItems)
            {
                if(p.Parent!=null && p.CalculationType==WfCalculationType.Manual)
                {
                    p.Progress = (p.WF/100) * p.Parent.Progress;
                }
            }
        }

        internal void ParentRicoveryWBS(ProjectWBS parent, List<ProjectWBS> childs)
        {
            childs.Add(parent);
            if (parent.Parent != null)
            {
                ParentRicoveryWBS(parent.Parent, childs);
            }
        }

        internal void ParentRicovery(ProjectWBS parent, List<WBSType> parents)
        {
            parents.Add(parent.Type);
            if (parent.Parent != null)
            {
                ParentRicovery(parent.Parent, parents);
            }
        }

        internal void getLastChildIds(ProjectWBS parent, List<long> childs)
        {
            if (parent.Childeren.Any())
            {
                foreach (var p in parent.Childeren)
                {
                    getLastChildIds(p, childs);
                }
            }
            else
            {
                if (!childs.Contains(parent.Id))
                    childs.Add(parent.Id);
            }
        }

        internal void ParentRicoveryToType(ProjectWBS parent, List<Tuple<long, WBSType>> childs)
        {
            childs.Add(new Tuple<long, WBSType>(parent.TargetId, parent.Type));
            if (parent.Parent != null)
            {
                ParentRicoveryToType(parent.Parent, childs);
            }
        }

        internal void geChilds(ProjectWBS parent, List<ProjectWBS> childs)
        {
            if (parent.Childeren.Any())
            {
                foreach (var p in parent.Childeren)
                {
                    if (!childs.Contains(parent))
                        childs.Add(parent);

                    geChilds(p, childs);
                }
            }
            else
            {
                if (!childs.Contains(parent))
                    childs.Add(parent);
            }
        }

        internal int childCount(ProjectWBS parent)
        {
            int counter = 1;
            if (parent.Childeren.Any())
            {
                foreach (var p in parent.Childeren)
                {
                    counter += childCount(p);
                }
            }
            return counter;
        }

        internal int ParetntCounter(ProjectWBS parent)
        {
            int counter = 1;
            if (parent.Parent != null)
            {
                counter += ParetntCounter(parent.Parent);
            }
            return counter;
        }

        internal void getActivityWbsTree(ProjectWBS parent, Dictionary<WBSType, long> targets, List<ProjectWBS> wbsTree)
        {
            if (parent.Childeren.Any())
            {
                foreach (var ch in parent.Childeren)
                {
                    if (targets.ContainsKey(ch.Type))
                    {
                        long rId = targets[ch.Type];
                        if (rId == ch.TargetId)
                        {
                            wbsTree.Add(ch);
                            getActivityWbsTree(ch, targets, wbsTree);
                        }
                    }
                }
            }
        }

        private IEnumerable<ActivityListDetailsDto> GetActivityForWbs(ProjectWBS sb, IEnumerable<ActivityListDetailsDto> activityes)
        {
            var lstParents = new List<ProjectWBS>();
            this.ParentRicoveryWBS(sb, lstParents);

            IEnumerable<ActivityListDetailsDto> lstItems = null;

            if (lstParents.Any(s => s.Type == WBSType.Project))
            {
                lstItems =activityes;
            }

            if (lstParents.Any(s => s.Type == WBSType.WorkPackage))
            {
                var lIds = lstParents.Where(s => s.Type == WBSType.WorkPackage).Select(s => s.TargetId).ToArray();
                lstItems = activityes.Where(s => lIds.Contains(s.WorkPackageId));
            }

            if (lstParents.Any(s => s.Type == WBSType.Location))
            {
                var lIds = lstParents.Where(s => s.Type == WBSType.Location).Select(s => s.TargetId).ToArray();
                lstItems = lstItems.Where(s => lIds.Contains(s.LocationId));
            }

            if (lstParents.Any(s => s.Type == WBSType.Descipline))
            {
                var lIds = lstParents.Where(s => s.Type == WBSType.Descipline).Select(s => s.TargetId).ToArray();
                lstItems = lstItems.Where(s => lIds.Contains(s.DesciplineId));
            }

            if (lstParents.Any(s => s.Type == WBSType.System))
            {
                var lIds = lstParents.Where(s => s.Type == WBSType.System).Select(s => s.TargetId).ToArray();
                lstItems = lstItems.Where(s => lIds.Contains(s.SystemdId));
            }

            if (lstParents.Any(s => s.Type == WBSType.SubSystem))
            {
                var lIds = lstParents.Where(s => s.Type == WBSType.SubSystem).Select(s => s.TargetId).ToArray();
                lstItems = lstItems.Where(s => lIds.Contains(s.SubsytemId));
            }

            return lstItems;
        }

    }
}
