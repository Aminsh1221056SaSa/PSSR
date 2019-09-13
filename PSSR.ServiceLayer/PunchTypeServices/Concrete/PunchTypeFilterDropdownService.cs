using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.PunchTypeServices.QueryObjects;
using PSSR.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.PunchTypeServices.Concrete
{
    public class PunchTypeFilterDropdownService
    {
        private readonly EfCoreContext _db;

        public PunchTypeFilterDropdownService(EfCoreContext db)
        {
            _db = db;
        }

        public IEnumerable<DropdownTuple> GetFilterDropDownValues(PunchTypeFilterBy filterBy)
        {
            switch (filterBy)
            {
                case PunchTypeFilterBy.NoFilter:
                    return new List<DropdownTuple>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
            }
        }

        private static IEnumerable<DropdownTuple> FormPrecentageDropDown()
        {
            return new[]
            {
                new DropdownTuple {Value = "100", Text = "100 % up"},
                new DropdownTuple {Value = "70", Text = "70 % up"},
                new DropdownTuple {Value = "30", Text = "30 % up"},
                new DropdownTuple {Value = "10", Text = "10 % up"},
            };
        }
    }
}
