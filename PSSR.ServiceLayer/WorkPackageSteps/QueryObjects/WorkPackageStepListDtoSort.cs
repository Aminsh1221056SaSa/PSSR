using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.WorkPackageSteps.QueryObjects
{
    public enum OrderByOptions
    {
        [Display(Name = "sort by...")]
        SimpleOrder = 0,
        [Display(Name = "Title")]
        ByName,
        [Display(Name = "Work Package")]
        ByWorkPackage
    }
    public static class WorkPackageStepListDtoSort
    {
        public static IQueryable<WorkPackageStepListDto> OrderWorkPackageStep
            (this IQueryable<WorkPackageStepListDto> workSteps,
             OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.SimpleOrder:
                    return workSteps.OrderByDescending(
                        x => x.Id);
                case OrderByOptions.ByName:
                    return workSteps.OrderByDescending(
                        x => x.Title);
                case OrderByOptions.ByWorkPackage:
                    return workSteps.OrderByDescending(
                        x => x.WorkPackageId);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}
