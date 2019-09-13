using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.SubSystemServices.QueryObjects
{
    public enum ProjectSubSystemFilterBy
    {
        [Display(Name = "All")]
        NoFilter = 0,
        [Display(Name = "By System")]
        ProjectSystem
    }

    public static class ProjectSubSystemListDtoFilter
    {
        public static IQueryable<ProjectSubSystemListDto> FilterProjectSubSystemBy(
             this IQueryable<ProjectSubSystemListDto> projectSubSystems,
             ProjectSubSystemFilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return projectSubSystems;

            switch (filterBy)
            {
                case ProjectSubSystemFilterBy.NoFilter:
                    return projectSubSystems;

                case ProjectSubSystemFilterBy.ProjectSystem:
                    var filterval = int.Parse(filterValue);
                    return projectSubSystems.Where(x =>
                          x.ProjectSystemId == filterval);
                default:
                    throw new ArgumentOutOfRangeException
                        (nameof(filterBy), filterBy, null);
            }
        }
    }
}
