using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PSSR.ServiceLayer.DesciplineServices.QueryObjects
{
    public enum OrderByOptions
    {
        [Display(Name = "sort by...")]
        SimpleOrder = 0,
        [Display(Name = "Title ↑")]
        ByTitle,
        [Display(Name = "Create Date ↑")]
        ByCreateDate,
        [Display(Name = "SubSystem Count ↓")]
        BySubSystemLowestFirst,
        [Display(Name = "SubSystem Count ↑")]
        BySubSystemHigestFirst
    }

    public static class DesciplineListDtoSort
    {
        public static IQueryable<DesciplineListDto> OrderDesciplineBy
            (this IQueryable<DesciplineListDto> desciplines,
             OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.SimpleOrder:
                    return desciplines.OrderByDescending(
                        x => x.Id);
                case OrderByOptions.ByTitle:
                    return desciplines.OrderByDescending(x =>
                        x.Title);
                case OrderByOptions.ByCreateDate:
                    return desciplines.OrderByDescending(
                        x => x.CreatedDate);
                case OrderByOptions.BySubSystemLowestFirst:
                    return desciplines.OrderBy(x => x.SubSystemCount);
                case OrderByOptions.BySubSystemHigestFirst:
                    return desciplines.OrderByDescending(
                        x => x.SubSystemCount);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}
    
