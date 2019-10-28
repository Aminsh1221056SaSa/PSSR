
using PSSR.Common.PersonService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.PersonService.QueryObjects
{
    public enum OrderByOptions
    {
        [Display(Name = "sort by...")]
        SimpleOrder = 0,
        [Display(Name = "FirstName")]
        ByFirstName,
        [Display(Name = "LastName")]
        ByLastName,
        [Display(Name = "NationalId")]
        ByNationalId
    }

    public static class PersonListDtoSort
    {
        public static IQueryable<PersonListDto> OrderPersonBy
           (this IQueryable<PersonListDto> Persons,
            OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.SimpleOrder:
                    return Persons.OrderByDescending(
                        x => x.Id);
                case OrderByOptions.ByFirstName:
                    return Persons.OrderBy(x =>
                        x.FirstName);
                case OrderByOptions.ByLastName:
                    return Persons.OrderBy(x =>
                        x.LastName);
                case OrderByOptions.ByNationalId:
                    return Persons.OrderBy(x =>
                        x.NationalId);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}
