using PSSR.ServiceLayer.ProjectSystemServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.SubSystemServices.QueryObjects
{
    public enum OrderByOptions
    {
        [Display(Name = "sort by...")]
        SimpleOrder = 0,
        [Display(Name = "Code")]
        ByCode,
        [Display(Name = "System")]
        BySystem
    }

    public static class ProjectSubSystemListDtoSort
    {
        public static IQueryable<ProjectSubSystemListDto> OrderProjectSubSystemBy
         (this IQueryable<ProjectSubSystemListDto> ProjectSubSystems,
          OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.SimpleOrder:
                    return ProjectSubSystems.OrderByDescending(
                        x => x.Id);
                case OrderByOptions.ByCode:
                    return ProjectSubSystems.OrderBy(x =>
                        x.Code);
                case OrderByOptions.BySystem:
                    return ProjectSubSystems.OrderBy(x =>
                        x.ProjectSystemId);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}
