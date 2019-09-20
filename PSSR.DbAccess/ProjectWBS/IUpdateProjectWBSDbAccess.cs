using PSSR.DataLayer.EfClasses.Projects;

namespace PSSR.DbAccess.Projects
{
    public interface IUpdateProjectWBSDbAccess
    {
        ProjectWBS GetProject(long projectwbsId);
    }
}
