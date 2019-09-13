using PSSR.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.ProjectServices.QueryObjects
{
    public enum ProjectFilterBy
    {
        [Display(Name = "All")]
        NoFilter = 0,
        [Display(Name = "By Type")]
        Type,
        [Display(Name = "By Contractor")]
        Contractory
    }

    public static class ProjectListDtoFilter
    {
        public static IQueryable<ProjectListDto> FilterProjectBy(
            this IQueryable<ProjectListDto> projects,
            ProjectFilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return projects;

            switch (filterBy)
            {
                case ProjectFilterBy.NoFilter:
                    return projects;

                case ProjectFilterBy.Type:
                    var filterval = filterValue.ParseEnum<ProjectType>();
                    return projects.Where(x =>
                          x.Type == filterval);

                case ProjectFilterBy.Contractory:
                    int contractorId = int.Parse(filterValue);
                    return projects.Where(x =>
                          x.ContractorId == contractorId);

                default:
                    throw new ArgumentOutOfRangeException
                        (nameof(filterBy), filterBy, null);
            }
        }
    }
}
