
using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.DbAccess.WorkPackages
{
    public interface IDeleteWorkPackageDbAccess
    {
        void Delete(WorkPackage projectwbs);
    }
}
