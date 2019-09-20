
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.Projects.Concrete
{
    public class UpdateProjectWBSDbAccess : IUpdateProjectWBSDbAccess
    {
        private readonly EfCoreContext _context;

        public UpdateProjectWBSDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public ProjectWBS GetProject(long projectwbsId)
        {
            return _context.Find<ProjectWBS>(projectwbsId);
        }
    }
}
