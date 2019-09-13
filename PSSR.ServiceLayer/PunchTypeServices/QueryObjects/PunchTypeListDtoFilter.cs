using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.PunchTypeServices.QueryObjects
{
    public enum PunchTypeFilterBy
    {
        [Display(Name = "All")]
        NoFilter = 0
    }

    public static class PunchTypeListDtoFilter
    {
        public static IQueryable<PunchTypeListDto> FilterPunchTypeBy(
             this IQueryable<PunchTypeListDto> punchTypes,
             PunchTypeFilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return punchTypes;

            switch (filterBy)
            {
                case PunchTypeFilterBy.NoFilter:
                    return punchTypes;

                default:
                    throw new ArgumentOutOfRangeException
                        (nameof(filterBy), filterBy, null);
            }
        }
    }
}
