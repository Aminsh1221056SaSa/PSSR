
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.Projects.Concrete
{
    public class PlaceProjectWBSDbAccess : IPlaceProjectWBSDbAccess
    {
        private readonly EfCoreContext _context;

        public PlaceProjectWBSDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Add(ProjectWBS projectwbs)
        {
            _context.ProjectWBS.Add(projectwbs);
        }
    }
}
