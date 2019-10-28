using PSSR.Common.ContractorServices;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PSSR.ServiceLayer.ContractorServices.QueryObjects
{
    public enum ContractorFilterBy
    {
        [Display(Name = "All")]
        NoFilter = 0
    }

    public static class ContractorListDtoFilter
    {
        public static IQueryable<ContractorListDto> FilterContractorBy(
          this IQueryable<ContractorListDto> contractors,
          ContractorFilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return contractors;

            switch (filterBy)
            {
                case ContractorFilterBy.NoFilter:
                    return contractors;
                default:
                    throw new ArgumentOutOfRangeException
                        (nameof(filterBy), filterBy, null);
            }
        }
    }
}
