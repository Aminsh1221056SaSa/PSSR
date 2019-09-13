using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.SubSystemServices.QueryObjects;
using PSSR.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSSR.ServiceLayer.SubSystemServices.Concrete
{
    public class ProjectSubSystemFilterDropdownService
    {
        private readonly EfCoreContext _db;

        public ProjectSubSystemFilterDropdownService(EfCoreContext db)
        {
            _db = db;
        }

        public IEnumerable<DropdownTuple> GetFilterDropDownValues(ProjectSubSystemFilterBy filterBy)
        {
            switch (filterBy)
            {
                case ProjectSubSystemFilterBy.NoFilter:
                    return new List<DropdownTuple>();

                case ProjectSubSystemFilterBy.ProjectSystem:
                    return _db.ProjectSystems.Select(s => new DropdownTuple
                    {
                        Text = s.Code,
                        Value = s.Id.ToString()
                    }).AsEnumerable();
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
            }
        }
        
    }
}
