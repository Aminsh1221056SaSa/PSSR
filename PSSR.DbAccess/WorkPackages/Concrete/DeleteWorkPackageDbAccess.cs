
using System.Collections.Generic;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.WorkPackages.Concrete
{
    public class DeleteWorkPackageDbAccess : IDeleteWorkPackageDbAccess
    {
        private readonly EfCoreContext _context;

        public DeleteWorkPackageDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Delete(WorkPackage projectwbs)
        {
            _context.ProjectRoadMaps.Remove(projectwbs);
        }
    }
}
