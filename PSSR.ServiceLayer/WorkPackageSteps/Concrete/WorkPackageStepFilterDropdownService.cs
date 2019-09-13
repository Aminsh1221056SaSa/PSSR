using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.Utils;
using PSSR.ServiceLayer.WorkPackageSteps.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.WorkPackageSteps.Concrete
{
    public class WorkPackageStepFilterDropdownService
    {
        private readonly EfCoreContext _db;

        public WorkPackageStepFilterDropdownService(EfCoreContext db)
        {
            _db = db;
        }

        public IEnumerable<DropdownTuple> GetFilterDropDownValues(WorkPackageStepFilterBy filterBy)
        {
            switch (filterBy)
            {
                case WorkPackageStepFilterBy.NoFilter:
                    return new List<DropdownTuple>();
                case WorkPackageStepFilterBy.WorkPackage:
                    return _db.ProjectRoadMaps.Select(s => new DropdownTuple
                    {
                        Text=s.Name,
                        Value=s.Id.ToString()
                    }).ToList();
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
            }
        }
    }
}
