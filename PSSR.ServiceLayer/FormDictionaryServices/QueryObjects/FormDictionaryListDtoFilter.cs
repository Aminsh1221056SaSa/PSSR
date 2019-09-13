using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using PSSR.Common;

namespace PSSR.ServiceLayer.FormDictionaryServices.QueryObjects
{
    public enum FormDictionaryFilterBy
    {
        [Display(Name = "All")]
        NoFilter = 0,
        [Display(Name = "By Type")]
        ByType,
        [Display(Name = "By Station Type")]
        StationType,
        [Display(Name = "By Descipline")]
        Descipline
    }

    public static class FormDictionaryListDtoFilter
    {
        public static IQueryable<FormDictionaryListDto> FilterFormDictionaryBy(
           this IQueryable<FormDictionaryListDto> formDictioanry,
           FormDictionaryFilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return formDictioanry;

            switch (filterBy)
            {
                case FormDictionaryFilterBy.NoFilter:
                    return formDictioanry;

                case FormDictionaryFilterBy.ByType:
                    var val = filterValue.ParseEnum<FormDictionaryType>();
                    return formDictioanry.Where(s => s.Type == val);
                    
                case FormDictionaryFilterBy.StationType:
                    var stVal =0;
                    int.TryParse(filterValue, out stVal);
                    return formDictioanry.Where(s => s.WorkPackageId == stVal);

                case FormDictionaryFilterBy.Descipline:
                    var desciplineId = 0;
                    int.TryParse(filterValue, out desciplineId);
                    return formDictioanry.Where(s => s.DesciplinesIds.Contains(desciplineId));

                default:
                    throw new ArgumentOutOfRangeException
                        (nameof(filterBy), filterBy, null);
            }
        }
    }
}
