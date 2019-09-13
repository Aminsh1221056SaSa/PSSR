
using PSSR.Common;
using System;

namespace PSSR.Logic.Activityes
{
    public class ActivityDto
    {
        public long Id { get;  set; }
        public string ActivityCode { get; set; }
        public string TagNumber { get;  set; }
        public string TagDescription { get;  set; }
        public float Progress { get;  set; }
        public float WeightFactor { get;  set; }
        public float ValueUnitNum { get;  set; }
        public int EstimateMh { get;  set; }
        public int ActualMh { get;  set; }
        public ActivityStatus Status { get;  set; }
        public ActivityCondition Condition { get; set; }
        public DateTime? ActualStartDate { get;  set; }
        public DateTime? ActualEndDate { get;  set; }
        public DateTime? PlanStartDate { get;  set; }
        public DateTime? PlanEndDate { get;  set; }
      
        public int ValueUnitId { get;  set; }
        public int WorkPackageId { get;  set; }
        public int LocationId { get;  set; }
        public long SubsytemId { get;  set; }
        public int DesciplineId { get;  set; }
        public long FormDictionaryId { get; set; }
        public int SystemdId { get; set; }
        public int WorkPackageStepId { get; set; }
    }
}
