using PSSR.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.ProjectSystemServices.QueryObjects
{
    public enum ProjectSystemFilterBy
    {
        [Display(Name = "All")]
        NoFilter = 0,
        [Display(Name = "By Type")]
        Type
    }

    public static class ProjectSystemListDtoFilter
    {
        public static IQueryable<ProjectSystemListDto> FilterProjectSystemBy(
             this IQueryable<ProjectSystemListDto> projectSystems,
             ProjectSystemFilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return projectSystems;

            switch (filterBy)
            {
                case ProjectSystemFilterBy.NoFilter:
                    return projectSystems;

                case ProjectSystemFilterBy.Type:
                    var filterval = filterValue.ParseEnum<SystemType>();
                    return projectSystems.Where(x =>
                          x.Type ==filterval);
                    
                default:
                    throw new ArgumentOutOfRangeException
                        (nameof(filterBy), filterBy, null);
            }
        }
    }
}
