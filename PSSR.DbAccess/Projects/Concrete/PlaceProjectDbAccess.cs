using PSSR.DataLayer.EfCode;
using PSSR.DataLayer.EfClasses.Projects;

namespace PSSR.DbAccess.Projects.Concrete
{
    public class PlaceProjectDbAccess:IPlaceProjectDbAccess
    {
        private readonly EfCoreContext _context;

        public PlaceProjectDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Add(Project project)
        {
            _context.Projects.Add(project);
        }
    }
}
