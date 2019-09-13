using PSSR.Common;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.MDRDocumentServices.QueryObjects;
using PSSR.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.MDRDocumentServices.Concrete
{
    public class MDRDocumentFilterDropdownService
    {
        private readonly EfCoreContext _db;

        public MDRDocumentFilterDropdownService(EfCoreContext db)
        {
            _db = db;
        }

        public IEnumerable<DropdownTuple> GetFilterDropDownValues(MDRDocumentFilterBy filterBy)
        {
            switch (filterBy)
            {
                case MDRDocumentFilterBy.NoFilter:
                    return new List<DropdownTuple>();
                case MDRDocumentFilterBy.ByStatus:
                    return _db.MDRStatus.Select(v => new DropdownTuple
                    {
                        Value = v.Id.ToString(),
                        Text = $"{v.Name}"
                    });
                case MDRDocumentFilterBy.ByWorkPackage:
                    return _db.ProjectRoadMaps.Select(v => new DropdownTuple
                        {
                            Value = v.Id.ToString(),
                            Text = $"{v.Name}"
                        });

                default:
                    throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
            }
        }
    }
}
