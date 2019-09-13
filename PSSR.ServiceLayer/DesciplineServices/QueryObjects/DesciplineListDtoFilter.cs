
using PSSR.DataLayer.EfClasses.Management;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PSSR.ServiceLayer.DesciplineServices.QueryObjects
{
    public enum DesiplineFilterBy
    {
        [Display(Name = "All")]
        NoFilter = 0,
        [Display(Name = "By Form Dictionary...")]
        ByFromDictionary
    }

    public static class DesciplineListDtoFilter
    {
        public static IQueryable<Descipline> FilterDesciplineBy(
            this IQueryable<Descipline> desciplines,
            DesiplineFilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return desciplines;

            switch (filterBy)
            {
                case DesiplineFilterBy.NoFilter:
                    return desciplines;
                case DesiplineFilterBy.ByFromDictionary:
                  
                    var filterYear = long.Parse(filterValue);
                    return desciplines.Where(
                        x => x.FormDictionaryLink.Any(s=>s.FormDictionaryId== filterYear));
                default:
                    throw new ArgumentOutOfRangeException
                        (nameof(filterBy), filterBy, null);
            }
        }

    }
}
