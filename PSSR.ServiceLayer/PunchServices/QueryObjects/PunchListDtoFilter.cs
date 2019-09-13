
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PSSR.ServiceLayer.PunchServices.QueryObjects
{
    public enum PunchFilterBy
    {
        [Display(Name = "All")]
        NoFilter = 0,
        [Display(Name = "By Type")]
        ByType,
        [Display(Name = "By WorkPackage")]
        ByWorkPackage,
        [Display(Name = "By Status")]
        ByStatus,
    }

    public static class PunchListDtoFilter
    {
        public static IQueryable<Punch> FilterPunchBy(
           this IQueryable<Punch> punches,
           PunchFilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return punches;

            switch (filterBy)
            {
                case PunchFilterBy.NoFilter:
                    return punches;

                case PunchFilterBy.ByType:
                    var punchtypeId = int.Parse(filterValue);
                    return punches.Where(
                        x => x.PunchTypeId == punchtypeId);

                case PunchFilterBy.ByWorkPackage:
                    var workPackageId = int.Parse(filterValue);
                    return punches.Where( x =>x.PunchType.WorkPackages.Any(s=>s.WorkPackageId==workPackageId));

                case PunchFilterBy.ByStatus:
                    var statusId = int.Parse(filterValue);
                    if(statusId==1)
                    {
                        return punches.Where(s => s.CheckDate.HasValue);
                    }
                    else if (statusId == 2)
                    {
                        return punches.Where(s => s.ClearDate.HasValue);
                    }
                    else
                    {
                        return punches;
                    }
                default:
                    throw new ArgumentOutOfRangeException
                        (nameof(filterBy), filterBy, null);
            }
        }
    }
}
