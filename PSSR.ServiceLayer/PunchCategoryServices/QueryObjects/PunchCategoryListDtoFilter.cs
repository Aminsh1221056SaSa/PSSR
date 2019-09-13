using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.PunchCategoryServices.QueryObjects
{
    public enum PunchCategoryFilterBy
    {
        [Display(Name = "All")]
        NoFilter = 0
    }

    public static class PunchCategoryListDtoFilter
    {
        public static IQueryable<PunchCategoryListDto> FilterPunchCategoryBy(
             this IQueryable<PunchCategoryListDto> punchTypes,
             PunchCategoryFilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return punchTypes;

            switch (filterBy)
            {
                case PunchCategoryFilterBy.NoFilter:
                    return punchTypes;

                default:
                    throw new ArgumentOutOfRangeException
                        (nameof(filterBy), filterBy, null);
            }
        }
    }
}
