using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PSSR.ServiceLayer.ActivityServices.QueryObjects
{
    public enum OrderByOptions
    {
        [Display(Name = "sort by...")]
        SimpleOrder = 0,
        [Display(Name = "Status")]
        ByStatus,
        [Display(Name = "Condition")]
        ByCondition,
        [Display(Name = "Progress ↓")]
        ByProgressLowestFirst,
        [Display(Name = "Progress ↑")]
        ByProgressHigestFirst,
        [Display(Name = "Punch Count")]
        ByPunchCount,
        [Display(Name = "Tag Number")]
        ByTagNumber,
        [Display(Name = "Wight Factor ↓")]
        ByWightFactorLowestFirst,
        [Display(Name = "Wight Factor ↑")]
        ByWightFactorHighestFirst,
        [Display(Name = "Actual Mh")]
        ByActualMh,
        [Display(Name = "Estimate Mh")]
        ByEstimateMh
    }

    public static class ActivityListDtoSort
    {
        public static IQueryable<ActivityListDto> OrderActivityBy
             (this IQueryable<ActivityListDto> activities,
              OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.SimpleOrder:
                    return activities.OrderByDescending(
                       x => x.Id);
                case OrderByOptions.ByStatus:
                    return activities.OrderBy(
                       x => x.Status);
                case OrderByOptions.ByCondition:
                    return activities.OrderBy(
                       x => x.Condition);
                case OrderByOptions.ByActualMh:
                    return activities.OrderByDescending(
                       x => x.ActualMh);
                case OrderByOptions.ByEstimateMh:
                    return activities.OrderByDescending(
                       x => x.EstimateMh);
                case OrderByOptions.ByProgressLowestFirst:
                    return activities.OrderBy(
                       x => x.Progress);
                case OrderByOptions.ByProgressHigestFirst:
                    return activities.OrderByDescending(
                       x => x.Progress);
                case OrderByOptions.ByPunchCount:
                    return activities.OrderByDescending(
                       x => x.PunchCount);
                case OrderByOptions.ByTagNumber:
                    return activities.OrderByDescending(
                       x => x.TagNumber);
                case OrderByOptions.ByWightFactorHighestFirst:
                    return activities.OrderByDescending(
                       x => x.WeightFactor);
                case OrderByOptions.ByWightFactorLowestFirst:
                    return activities.OrderBy(
                       x => x.WeightFactor);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}
