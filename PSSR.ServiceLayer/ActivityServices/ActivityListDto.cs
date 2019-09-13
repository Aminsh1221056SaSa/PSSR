
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using PSSR.Common;
using System;
using System.ComponentModel;

namespace PSSR.ServiceLayer.ActivityServices
{
    public class ActivityListDto
    {
        public long Id { get; set; }

        public string ActivityCode { get; set; }

        public string TagNumber { get; set; }

        public float Progress { get; set; }

        public float WeightFactor { get; set; }

        public int EstimateMh { get; set; }

        public int ActualMh { get; set; }

        public ActivityStatus Status { get; set; }

        public ActivityCondition Condition { get; set; }

        public int PunchCount { get; set; }

        public FormDictionaryType FormType { get; set; }

        public long FormDictionaryId { get; set; }

        public string PlanStartDate { get; set; }

        public string PlanEndDate { get; set; }
    }

    public class ActivityListDetailsDto
    {
        public long Id { get; set; }
        public string ActivityCode { get; set; }
        public string TagNumber { get; set; }
        public float WeightFactor { get; set; }
        public float ValueUnitNum { get; set; }
        public float Progress { get; set; }
        public int EstimateMh { get; set; }
        public int ActualMh { get; set; }
        public ActivityStatus Status { get; set; }
        public ActivityCondition Condition { get; set; }
        public string ActualStartDate { get; set; }
        public string ActualEndDate { get; set; }
        public string PlanStartDate { get; set; }
        public string PlanEndDate { get; set; }
        public string TagDescription { get; set; }

        public long? FormDictionaryId { get; set; }
        public int ValueUnitId { get; set; }
        public int WorkPackageId { get;  set; }
        public int LocationId { get;  set; }
        public int SystemdId { get; set; }
        public long SubsytemId { get;  set; }
        public int DesciplineId { get;  set; }
        public int WorkPackageStepId { get; set; }
        public string WorkPackageName { get; set; }
        public string LocationName { get; set; }
        public string DesciplineName { get; set; }
        public string SystemName { get; set; }
        public string SubSystemName { get; set; }
        public string FormCode { get; set; }
        public string FormDescription { get; set; }
        public string FormType { get; set; }
        public string WorkPackageStepName { get; set; }
        public DateTime? RPlanStartDate { get; set; }
        public DateTime? RPlanEndDate { get; set; }
    }

    public class ActivityListPlanDto
    {
        public long Id { get; set; }
        public string ActivityCode { get; set; }
        public string TagNumber { get; set; }
        public float WeightFactor { get; set; }
        public ActivityStatus Status { get; set; }
        public DateTime PlanStartDate { get; set; }
        public DateTime PlanEndDate { get; set; }
        public long FormDictionaryId { get; set; }
        public int ValueUnitId { get; set; }
        public int WorkPackageId { get; set; }
        public int LocationId { get; set; }
        public int SystemdId { get; set; }
        public long SubsytemId { get; set; }
        public int DesciplineId { get; set; }
        public int WorkPackageStepId { get; set; }
    }

    public class ActivityListDataTableDto
    {
        [DisplayName("Details")]
        public long Id { get; set; }

        [SearchableString]
        [Sortable(Default = true)]
        public string ActivityCode { get; set; }

        [SearchableString]
        [Sortable]
        public string TagNumber { get; set; }

        [SearchableInt]
        [Sortable(Default = true)]
        public string Status { get; set; }

        [SearchableInt]
        [Sortable]
        public string Condition { get; set; }

        [Sortable]
        [SearchableDouble]
        public float Progress { get; set; }

        //workpackage
        [Sortable]
        public string WorkPackage { get; set; }
        [SearchableInt]
        public int WorkPackageId { get; set; }

        //location
        [Sortable]
        public string Location { get; set; }
        [SearchableInt]
        public int LocationId { get; set; }

        //descipline
        [Sortable]
        public string Descipline { get; set; }
        [SearchableInt]
        public int DesciplineId { get; set; }

        //system
        [Sortable(EntityProperty = "SubSystem.ProjectSystemId")]
        public string System { get; set; }
        [SearchableDouble(EntityProperty = "SubSystem.ProjectSystemId")]
        public long SystemId { get; set; }

        //subsystem
        [Sortable]
        public string SubSystem { get; set; }
        [SearchableDouble]
        public long SubsytemId { get; set; }

        //FormDictionary
        [Sortable]
        public string FormDictionary { get; set; }

        [SearchableInt(EntityProperty = "FormDictionary.Type")]
        [Sortable(EntityProperty = "FormDictionary.Type")]
        public string FormType { get; set; }

        [SearchableDouble]
        public long FormDictionaryId { get; set; }

        //workpackagestep
        [Sortable]
        public string WorkPackageStep { get; set; }
        [SearchableInt]
        public int WorkPackageStepId { get; set; }

        [SearchableDouble]
        [Sortable]
        public float WeightFactor { get; set; }

        [SearchableInt]
        [Sortable]
        public int EstimateMh { get; set; }

        [SearchableInt]
        [Sortable(Default = true)]
        public int ActualMh { get; set; }

        public int PunchCount { get; set; }

        //datetime
        [SearchableDateTime]
        [Sortable]
        public DateTime? PlanStartDate { get; set; }

        [SearchableDateTime]
        [Sortable]
        public DateTime? PlanEndDate { get; set; }

        [SearchableDateTime]
        [Sortable]
        public DateTime? ActualStartDate { get; set; }

        [SearchableDateTime]
        [Sortable]
        public DateTime? ActualEndDate { get; set; }

        [SearchableString]
        [Sortable]
        public string ValueUnit { get; set; }
        [SearchableInt]
        public long ValueUnitId { get; set; }

        //value unit
        [SearchableShort]
        [Sortable]
        public float ValueUnitNum { get; set; }
    }

    public class ActivityListExcelDataTableDto
    {
        public string ActivityCode { get; set; }

        public string TagNumber { get; set; }

        public string Status { get; set; }

        public float Progress { get; set; }

        public string WorkPackage { get; set; }

        public string Location { get; set; }

        public string Descipline { get; set; }
        public string System { get; set; }

        public string SubSystem { get; set; }
        public string FormDictionary { get; set; }
        public string FormType { get; set; }
        public string WorkPackageStep { get; set; }
        public float WeightFactor { get; set; }
        public int EstimateMh { get; set; }

        public int ActualMh { get; set; }

        public int PunchCount { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string ValueUnit { get; set; }
        public float ValueUnitNum { get; set; }
        public string Condition { get; set; }
    }
}
