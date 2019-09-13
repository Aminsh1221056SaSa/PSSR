using PSSR.Common;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PSSR.ServiceLayer.ActivityServices.QueryObjects
{
    public enum ActivityFilterBy
    {
        [Display(Name = "All")]
        NoFilter = 0,
        [Display(Name = "By Status")]
        ByStatus,
        [Display(Name = "By Condition")]
        ByCondition,
        //[Display(Name = "By Work Package")]
        //ByWorkPackage,
        [Display(Name = "By Work Package Step")]
        ByWorkPackageStep,
        //[Display(Name = "By Location")]
        //ByLocation,
        [Display(Name = "By Descipline")]
        ByDescipline,
        //[Display(Name = "By System")]
        //BySystem,
        //[Display(Name = "By SubSystem")]
        //BySubSystem,
        [Display(Name = "By FormDictionary")]
        ByFormDictionary,
        [Display(Name = "By Form Type")]
        ByFormType,
        [Display(Name = "By Progress")]
        ByProgress
    }

    public static class ActivityListDtoFilter
    {
        public static IQueryable<Activity> FilterActivityBy(
           this IQueryable<Activity> activites,
           ActivityFilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return activites;

            switch (filterBy)
            {
                case ActivityFilterBy.NoFilter:
                    return activites;

                case ActivityFilterBy.ByStatus:
                    var status = filterValue.ParseEnum<ActivityStatus>();
                    return activites.Where(
                        x => x.Status == status);

                case ActivityFilterBy.ByCondition:
                    var condition = filterValue.ParseEnum<ActivityCondition>();
                    return activites.Where(
                        x => x.Condition == condition);

                case ActivityFilterBy.ByDescipline:
                    var desciplineId = int.Parse(filterValue);
                    return activites.Where(
                        x => x.FormDictionary.DesciplineLink.Any(s=>s.DesciplineId==desciplineId));

                case ActivityFilterBy.ByFormType:
                    var filterval1 = filterValue.ParseEnum<FormDictionaryType>();
                    return activites.Where(
                        x => x.FormDictionary.Type ==filterval1);

                //case ActivityFilterBy.ByWorkPackage:
                //    var workPackageId = int.Parse(filterValue);
                //    return activites.Where(
                //        x => x.WorkPackageId==workPackageId);

                case ActivityFilterBy.ByWorkPackageStep:
                    var workPackageStepId = int.Parse(filterValue);
                    return activites.Where(
                        x => x.WorkPackageStepId == workPackageStepId);

                //case ActivityFilterBy.ByLocation:
                //    var locationId = int.Parse(filterValue);
                //    return activites.Where(x => x.LocationId == locationId);

                //case ActivityFilterBy.BySystem:
                //    var systemId = int.Parse(filterValue);
                //    return activites.Where(x => x.SubSystem.ProjectSystemId == systemId);

                //case ActivityFilterBy.BySubSystem:
                //    var subSystemId = int.Parse(filterValue);
                //    return activites.Where(x => x.SubsytemId == subSystemId);

                case ActivityFilterBy.ByFormDictionary:
                    var formDicId = int.Parse(filterValue);
                    return activites.Where(x => x.FormDictionaryId == formDicId);

                case ActivityFilterBy.ByProgress:
                    var progress = float.Parse(filterValue);
                    return activites.Where(x => x.Progress >= progress);

                default:
                    throw new ArgumentOutOfRangeException
                        (nameof(filterBy), filterBy, null);
            }
        }
    }
}
