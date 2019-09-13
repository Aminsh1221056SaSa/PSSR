
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.Projects.Concrete
{
    public class DeleteProjectWBSDbAccess : IDeleteProjectWBSDbAccess
    {
        private readonly EfCoreContext _context;

        public DeleteProjectWBSDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Delete(ProjectWBS projectwbs)
        {
            _context.ProjectWBS.Remove(projectwbs);
        }
    }
}
