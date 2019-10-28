using PSSR.Common.ContractorServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.ContractorServices.QueryObjects
{
    public enum OrderByOptions
    {
        [Display(Name = "sort by...")]
        SimpleOrder = 0,
        [Display(Name = "Phone Number")]
        ByPhoneNumber
    }

    public static class ContractorListDtoSort
    {
        public static IQueryable<ContractorListDto> OrderContractorBy
          (this IQueryable<ContractorListDto> contractors,
           OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.SimpleOrder:
                    return contractors.OrderBy(x => x.Id);
                case OrderByOptions.ByPhoneNumber:
                    return contractors.OrderBy(x => x.PhoneNumber);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}
