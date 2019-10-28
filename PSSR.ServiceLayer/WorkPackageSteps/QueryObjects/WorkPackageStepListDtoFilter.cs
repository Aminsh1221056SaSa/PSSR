using PSSR.Common.WorkPackageSteps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.WorkPackageSteps.QueryObjects
{
    public enum WorkPackageStepFilterBy
    {
        [Display(Name = "All")]
        NoFilter = 0,
        [Display(Name = "ByWorkPackage")]
        WorkPackage = 1,
    }

    public static class WorkPackageStepListDtoFilter
    {
        public static IQueryable<WorkPackageStepListDto> FilterWorkPackageStepBy(
             this IQueryable<WorkPackageStepListDto> workSteps,
             WorkPackageStepFilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return workSteps;

            switch (filterBy)
            {
                case WorkPackageStepFilterBy.NoFilter:
                    return workSteps;
                case WorkPackageStepFilterBy.WorkPackage:
                    var workPackageId = int.Parse(filterValue);
                    return workSteps.Where(s => s.WorkPackageId == workPackageId);
                default:
                    throw new ArgumentOutOfRangeException
                        (nameof(filterBy), filterBy, null);
            }
        }
    }
}
