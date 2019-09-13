using PSSR.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.MDRDocumentServices.QueryObjects
{
    public enum MDRDocumentFilterBy
    {
        [Display(Name = "All")]
        NoFilter = 0,
        [Display(Name = "By Status")]
        ByStatus,
        [Display(Name = "By Work Package")]
        ByWorkPackage
    }

    public static class MDRDocumentListDtoFilter
    {
        public static IQueryable<MDRDocumentListDto> FilterMDRDocumentBy(
           this IQueryable<MDRDocumentListDto> mdrDocuments,
           MDRDocumentFilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return mdrDocuments;

            switch (filterBy)
            {
                case MDRDocumentFilterBy.NoFilter:
                    return mdrDocuments;

                case MDRDocumentFilterBy.ByStatus:
                    var statusVal = Convert.ToInt32(filterValue);
                    return mdrDocuments.Where(s => s.LastStatusId == statusVal);
                case MDRDocumentFilterBy.ByWorkPackage:
                    var stVal =Convert.ToInt64(filterValue);
                    return mdrDocuments.Where(s => s.WorkPackageId == stVal);

                default:
                    throw new ArgumentOutOfRangeException
                        (nameof(filterBy), filterBy, null);
            }
        }
    }
}
