using PSSR.DataLayer.EfClasses.Projects;

namespace PSSR.DbAccess.Projects
{
    public  interface IDeleteProjectWBSDbAccess
    {
        void Delete(ProjectWBS projectwbs);
    }
}
