
using PSSR.Common.FormDictionaryServices;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PSSR.ServiceLayer.FormDictionaryServices.QueryObjects
{
    public enum OrderByOptions
    {
        [Display(Name = "sort by...")]
        SimpleOrder = 0,
        [Display(Name = "Code")]
        ByCode,
        [Display(Name = "Type")]
        ByType,
        [Display(Name = "Station Type")]
        ByStationType,
        [Display(Name = "Priority ↓")]
        ByPriority,
        [Display(Name = "Priority ↑")]
        ByPriorityDesc
    }

    public static class FormDictionaryListDtoSort
    {
        public static IQueryable<FormDictionaryListDto> OrderFormDictionaryBy
              (this IQueryable<FormDictionaryListDto> formDics,
               OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.SimpleOrder:
                    return formDics.OrderByDescending(
                       x => x.Id);
                case OrderByOptions.ByCode:
                    return formDics.OrderBy(
                       x => x.Code);
                case OrderByOptions.ByStationType:
                    return formDics.OrderBy(
                       x => x.WorkPackageId);
                case OrderByOptions.ByType:
                    return formDics.OrderBy(
                       x => x.Type);

                case OrderByOptions.ByPriority:
                    return formDics.OrderBy(
                       x => x.Priority);

                case OrderByOptions.ByPriorityDesc:
                    return formDics.OrderByDescending(
                       x => x.Priority);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}
