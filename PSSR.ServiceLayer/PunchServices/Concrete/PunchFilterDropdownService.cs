using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.PunchServices.QueryObjects;
using PSSR.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.PunchServices.Concrete
{
    public class PunchFilterDropdownService
    {
        private readonly EfCoreContext _db;

        public PunchFilterDropdownService(EfCoreContext db)
        {
            _db = db;
        }

        public IEnumerable<DropdownTuple> GetFilterDropDownValues(PunchFilterBy filterBy)
        {
            switch (filterBy)
            {
                case PunchFilterBy.ByStatus:
                    return PunchStatusDropDown();
                case PunchFilterBy.ByType:
                    return _db.PunchTypes
                        .Select(s => new DropdownTuple
                        {
                            Value = s.Id.ToString(),
                            Text = s.Name
                        }).ToList();
                case PunchFilterBy.ByWorkPackage:
                    return _db.ProjectRoadMaps
                        .Select(s => new DropdownTuple
                        {
                            Value = s.Id.ToString(),
                            Text = s.Name
                        }).ToList();
                case PunchFilterBy.NoFilter:
                    return new List<DropdownTuple>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
            }
        }

        private static IEnumerable<DropdownTuple> PunchStatusDropDown()
        {
            return new[]
            {
                new DropdownTuple {Value = "2", Text = "Cleared"},
                new DropdownTuple {Value = "1", Text = "Checked"},
            };
        }
    }
}
