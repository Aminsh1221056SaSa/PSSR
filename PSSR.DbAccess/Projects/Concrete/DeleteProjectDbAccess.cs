
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.Projects.Concrete
{
    public class DeleteContractorDbAccess : IDeleteProjectDbAccess
    {
        private readonly EfCoreContext _context;

        public DeleteContractorDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Delete(Project project)
        {
            _context.Projects.Remove(project);
        }
    }
}
