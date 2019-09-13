
using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.DbAccess.WorkPackageSteps
{
    public interface IUpdateWorkPackageStepDbAccess
    {
        WorkPackageStep GetWorkPackageStep(int workPackagestepId);
        bool HaveAnyActivity(int id);
    }
}
