
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System.Collections.Generic;
using System.Linq;

namespace PSSR.ServiceLayer.ActivityServices.QueryObjects
{
    public static class ActivityListDtoSelect
    {
        public static IQueryable<ActivityListDto>MapActivityToDto(this IQueryable<Activity> activities,int workId,
            int locationId,int systemId,long subSystemId)
        {
            if (workId > 0)
            {
                activities = activities.Where(s => s.WorkPackageId == workId);
            }

            if (locationId > 0)
            {
                activities = activities.Where(s => s.LocationId == locationId);
            }

            if(systemId>0)
            {
                activities= activities.Where(s => s.SubSystem.ProjectSystemId == systemId);
            }

            if (subSystemId > 0)
            {
                activities = activities.Where(s => s.SubsytemId == subSystemId);
            }

            return activities.Select(p => new ActivityListDto
            {
                ActualMh = p.ActualMh,
                EstimateMh = p.EstimateMh,
                FormDictionaryId = p.FormDictionaryId,
                Id = p.Id,
                Progress = p.Progress,
                PunchCount = p.Punchs.Count,
                Status = p.Status,
                TagNumber = p.TagNumber,
                WeightFactor = p.WeightFactor,
                Condition = p.Condition,
                ActivityCode = p.ActivityCode,
            });

        }

        public static IQueryable<ActivityListDto> MapActivityToDtoWithDateTime(this IQueryable<Activity> activities)
        {
            return activities.Select(p => new ActivityListDto
            {
                ActualMh = p.ActualMh,
                EstimateMh = p.EstimateMh,
                FormDictionaryId = p.FormDictionaryId,
                Id = p.Id,
                Progress = p.Progress,
                PunchCount = p.Punchs.Count,
                Status = p.Status,
                TagNumber = p.TagNumber,
                WeightFactor = p.WeightFactor,
                Condition = p.Condition,
                ActivityCode = p.ActivityCode,
                PlanStartDate = p.PlanStartDate != null ? p.PlanStartDate.Value.ToString("d") : "",
                PlanEndDate = p.PlanEndDate != null ? p.PlanEndDate.Value.ToString("d") : "",
            });
        }

        public static IQueryable<ActivityListDataTableDto> MapActivityToDtoDataTable(this IQueryable<Activity> activities)
        {
            return activities.Select(p => new ActivityListDataTableDto
            {
                FormType=p.FormDictionary.Type.ToString(),
                ActualMh = p.ActualMh,
                EstimateMh = p.EstimateMh,
                Id = p.Id,
                Progress = p.Progress,
                PunchCount = p.Punchs.Count,
                Status = p.Status.ToString(),
                TagNumber = p.TagNumber,
                WeightFactor = p.WeightFactor,
                Condition = p.Condition.ToString(),
                ActivityCode = p.ActivityCode,
                PlanStartDate = p.PlanStartDate,
                PlanEndDate = p.PlanEndDate,
                ActualEndDate=p.ActualEndDate,
                ActualStartDate=p.ActualStartDate,
                Descipline=p.Descipline.Name,
                FormDictionary=p.FormDictionary.Code,
                Location=p.Location.Title,
                SubSystem=p.SubSystem.Code,
                WorkPackage=p.WorkPackage.Name,
                System=p.SubSystem.ProjectSystem.Code,
                WorkPackageStep=p.WorkPackageStep.Title,
                ValueUnit=p.ValueUnit.Name,
                ValueUnitNum=p.ValueUnitNum,
                WorkPackageId=p.WorkPackageId,
                DesciplineId=p.DesciplineId,
                LocationId=p.LocationId,
                SystemId=p.SubSystem.ProjectSystemId,
                FormDictionaryId=p.FormDictionaryId,
                SubsytemId = p.SubsytemId,
                WorkPackageStepId=p.WorkPackageStepId
            });
        }

        public static IEnumerable<ActivityListExcelDataTableDto> MapActivityToExcelDtoDataTable(this IEnumerable<ActivityListDataTableDto> activities)
        {
            return activities.Select(p => new ActivityListExcelDataTableDto
            {
                ActualMh = p.ActualMh,
                EstimateMh = p.EstimateMh,
                Progress = p.Progress,
                PunchCount = p.PunchCount,
                Status = p.Status,
                TagNumber = p.TagNumber,
                WeightFactor = p.WeightFactor,
                Condition = p.Condition,
                ActivityCode = p.ActivityCode,
                PlanStartDate = p.PlanStartDate,
                PlanEndDate = p.PlanEndDate,
                ActualEndDate = p.ActualEndDate,
                ActualStartDate = p.ActualStartDate,
                ValueUnitNum = p.ValueUnitNum,
                Descipline = p.Descipline,
                FormDictionary = p.FormDictionary,
                Location = p.Location,
                SubSystem = p.SubSystem,
                WorkPackage = p.WorkPackage,
                System = p.System,
                WorkPackageStep = p.WorkPackageStep,
                ValueUnit = p.ValueUnit,
                FormType=p.FormType
            });
        }
    }
}
