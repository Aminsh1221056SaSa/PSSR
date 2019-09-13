using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.PunchCategoryServices.QueryObjects
{
    public enum OrderByOptions
    {
        [Display(Name = "sort by...")]
        SimpleOrder = 0,
        [Display(Name = "Name")]
        ByName
    }

    public static class PunchCategoryListDtoSort
    {
        public static IQueryable<PunchCategoryListDto> OrderPunchCategoryBy
            (this IQueryable<PunchCategoryListDto> punchTypes,
             OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.SimpleOrder:
                    return punchTypes.OrderByDescending(
                        x => x.Id);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}
