using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.MDRDocumentServices.QueryObjects
{
    public enum OrderByOptions
    {
        [Display(Name = "sort by...")]
        SimpleOrder = 0,
        [Display(Name = "Status")]
        ByStatus,
        [Display(Name = "Create Date")]
        ByCreateDate,
        [Display(Name = "Update Date")]
        ByUpdateDate
    }

    public static class MDRDocumentListDtoSort
    {
        public static IQueryable<MDRDocumentListDto> OrderMDRDocumentBy
                (this IQueryable<MDRDocumentListDto> mdrDocuments,
                 OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.SimpleOrder:
                    return mdrDocuments.OrderByDescending(
                       x => x.Id);
                case OrderByOptions.ByCreateDate:
                    return mdrDocuments.OrderByDescending(
                       x => x.CreatedDate);
                case OrderByOptions.ByUpdateDate:
                    return mdrDocuments.OrderByDescending(
                       x => x.UpdatedDate);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}
