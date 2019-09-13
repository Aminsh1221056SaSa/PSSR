using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.DesciplineServices.QueryObjects;
using PSSR.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.DesciplineServices.Concrete
{
    public class DesciplineFilterDropdownService
    {
        private readonly EfCoreContext _db;

        public DesciplineFilterDropdownService(EfCoreContext db)
        {
            _db = db;
        }

        public IEnumerable<DropdownTuple> GetFilterDropDownValues(DesiplineFilterBy filterBy)
        {
            switch (filterBy)
            {
                case DesiplineFilterBy.NoFilter:
                    return new List<DropdownTuple>();

                case DesiplineFilterBy.ByFromDictionary:
                   return _db.FormDictionaries.Select(v => new DropdownTuple
                    {
                        Value = v.Id.ToString(),
                        Text =$" {v.Code} ({v.Description})"
                    });
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
            }
        }

        private static IEnumerable<DropdownTuple> FormSubSystemDropDown()
        {
            return new[]
            {
                new DropdownTuple {Value = "50", Text = "50 sub system and up"},
                new DropdownTuple {Value = "30", Text = "30 sub system and up"},
                new DropdownTuple {Value = "15", Text = "15 sub system and up"},
                new DropdownTuple {Value = "5", Text = "5 sub system and up"},
            };
        }
    }
}
