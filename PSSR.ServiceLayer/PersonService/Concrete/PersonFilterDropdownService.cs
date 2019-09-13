using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.PersonService.QueryObjects;
using PSSR.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.PersonService.Concrete
{
    public class PersonFilterDropdownService
    {
        private readonly EfCoreContext _db;

        public PersonFilterDropdownService(EfCoreContext db)
        {
            _db = db;
        }

        public IEnumerable<DropdownTuple> GetFilterDropDownValues(PersonFilterBy filterBy)
        {
            switch (filterBy)
            {
                case PersonFilterBy.NoFilter:
                    return new List<DropdownTuple>();

                case PersonFilterBy.Project:
                    return _db.Projects.Select(s => new DropdownTuple
                    {
                        Text=s.Description,
                        Value=s.Id.ToString()
                    });

                default:
                    throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
            }
        }
    }
}
