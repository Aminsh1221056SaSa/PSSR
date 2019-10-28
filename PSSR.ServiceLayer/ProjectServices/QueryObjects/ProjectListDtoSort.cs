using PSSR.Common.ProjectServices;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PSSR.ServiceLayer.ProjectServices.QueryObjects
{
    public enum ProjectOrderByOptions
    {
        [Display(Name = "sort by...")]
        SimpleOrder = 0,
        [Display(Name = "Start Date")]
        ByStartDate,
        [Display(Name = "End Date")]
        ByEndDate,
        [Display(Name = "Type")]
        ByType
    }

    public static class ProjectSystemListDtoSort
    {
        public static IQueryable<ProjectListDto> OrderProjectBy
           (this IQueryable<ProjectListDto> Projects,
            ProjectOrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case ProjectOrderByOptions.SimpleOrder:
                    return Projects.OrderByDescending(
                        x => x.Id);
                case ProjectOrderByOptions.ByEndDate:
                    return Projects.OrderByDescending(x =>
                        x.EndDate);
                case ProjectOrderByOptions.ByStartDate:
                    return Projects.OrderByDescending(x =>
                        x.StartDate);
                case ProjectOrderByOptions.ByType:
                    return Projects.OrderBy(x =>
                        x.Type);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}
