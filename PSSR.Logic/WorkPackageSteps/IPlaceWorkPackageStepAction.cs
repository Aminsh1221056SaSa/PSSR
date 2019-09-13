using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.Logic.WorkPackageSteps
{
    public interface IPlaceWorkStepPackageAction : IGenericActionWriteDb<WorkPackageStepDto, WorkPackageStep>
    {
    }
}
