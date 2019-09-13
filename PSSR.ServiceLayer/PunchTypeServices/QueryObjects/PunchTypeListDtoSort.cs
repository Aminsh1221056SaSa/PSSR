using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.PunchTypeServices.QueryObjects
{
    public enum OrderByOptions
    {
        [Display(Name = "sort by...")]
        SimpleOrder = 0,
        [Display(Name = "Name")]
        ByName
    }

    public static class PunchTypeListDtoSort
    {
        public static IQueryable<PunchTypeListDto> OrderPunchTypeBy
            (this IQueryable<PunchTypeListDto> punchTypes,
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
