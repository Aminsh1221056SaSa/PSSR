

using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.DbAccess.WorkPackageSteps
{
    public interface IDeleteWorkPackageStepDbAccess
    {
        void Delete(WorkPackageStep workPackageStep);
    }
}
