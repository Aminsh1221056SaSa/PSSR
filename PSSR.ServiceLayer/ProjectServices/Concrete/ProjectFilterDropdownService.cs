using PSSR.Common;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ProjectServices.QueryObjects;
using PSSR.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.ProjectServices.Concrete
{
    public class ProjectFilterDropdownService
    {
        private readonly EfCoreContext _db;

        public ProjectFilterDropdownService(EfCoreContext db)
        {
            _db = db;
        }

        public IEnumerable<DropdownTuple> GetFilterDropDownValues(ProjectFilterBy filterBy)
        {
            switch (filterBy)
            {
                case ProjectFilterBy.NoFilter:
                    return new List<DropdownTuple>();

                case ProjectFilterBy.Type:
                    return Enum.GetValues(typeof(ProjectType))
                       .Cast<ProjectType>().Select(v => new DropdownTuple
                       {
                           Value = v.ToString(),
                           Text = v.ToString()
                       });

                case ProjectFilterBy.Contractory:
                    return _db.Contractors.Select(v => new DropdownTuple
                       {
                           Value = v.Id.ToString(),
                           Text = v.Name.ToString()
                       });
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
            }
        }
    }
}
