using PSSR.Common;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.ProjectSystemServices.QueryObjects;
using PSSR.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.ProjectSystemServices.Concrete
{
    public class ProjectSystemFilterDropdownService
    {
        private readonly EfCoreContext _db;

        public ProjectSystemFilterDropdownService(EfCoreContext db)
        {
            _db = db;
        }

        public IEnumerable<DropdownTuple> GetFilterDropDownValues(ProjectSystemFilterBy filterBy)
        {
            switch (filterBy)
            {
                case ProjectSystemFilterBy.NoFilter:
                    return new List<DropdownTuple>();

                case ProjectSystemFilterBy.Type:
                    return Enum.GetValues(typeof(SystemType))
                       .Cast<SystemType>().Select(v => new DropdownTuple
                       {
                           Value = v.ToString(),
                           Text = v.ToString()
                       });
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
            }
        }
        
    }
}
