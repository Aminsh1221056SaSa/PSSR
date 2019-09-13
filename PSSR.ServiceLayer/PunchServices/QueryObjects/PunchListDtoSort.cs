
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PSSR.ServiceLayer.PunchServices.QueryObjects
{
    public enum OrderByOptions
    {
        [Display(Name = "sort by...")]
        SimpleOrder = 0,
        [Display(Name = "Type")]
        ByType,
        [Display(Name = "Orginated Date ↓")]
        ByOrginatedDateLowestFirst,
        [Display(Name = "Orginated Date ↑")]
        ByOrginatedDateHighestFirst,
        [Display(Name = "Clear Date ↓")]
        ByClearDateLowestFirst,
        [Display(Name = "Clear Date ↑")]
        ByClearDateHighestFirst
    }

    public static class PunchListDtoSort
    {
        public static IQueryable<Punch> OrderPunchBy
          (this IQueryable<Punch> punches,
           OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.SimpleOrder:
                    return punches.OrderByDescending(
                       x => x.Id);
                case OrderByOptions.ByType:
                    return punches.OrderBy(
                       x => x.PunchTypeId);
                case OrderByOptions.ByOrginatedDateLowestFirst:
                    return punches.OrderBy(
                       x => x.OrginatedDate);
                case OrderByOptions.ByOrginatedDateHighestFirst:
                    return punches.OrderByDescending(
                       x => x.OrginatedDate);
                case OrderByOptions.ByClearDateHighestFirst:
                    return punches.OrderByDescending(
                       x => x.ClearDate);
                case OrderByOptions.ByClearDateLowestFirst:
                    return punches.OrderBy(
                       x => x.ClearDate);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}
