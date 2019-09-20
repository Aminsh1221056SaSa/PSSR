
using PSSR.DataLayer.EfClasses.Projects;

namespace PSSR.DbAccess.Projects
{
    public interface IDeleteProjectDbAccess
    {
        void Delete(Project project);
    }
}
