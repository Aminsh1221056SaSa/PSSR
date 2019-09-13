using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.ProjectSystemServices.QueryObjects
{
    public enum OrderByOptions
    {
        [Display(Name = "sort by...")]
        SimpleOrder = 0,
        [Display(Name = "Code")]
        ByCode,
        [Display(Name = "Type")]
        ByType
    }

    public static class ProjectSystemListDtoSort
    {
        public static IQueryable<ProjectSystemListDto> OrderProjectSystemBy
           (this IQueryable<ProjectSystemListDto> ProjectSystems,
            OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.SimpleOrder:
                    return ProjectSystems.OrderByDescending(
                        x => x.Id);
                case OrderByOptions.ByCode:
                    return ProjectSystems.OrderBy(x =>
                        x.Code);
                case OrderByOptions.ByType:
                    return ProjectSystems.OrderBy(x =>
                        x.Type);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}
