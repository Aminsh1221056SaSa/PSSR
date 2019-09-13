using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DbAccess.RoadMaps;
using PSSR.DbAccess.WorkPackageSteps;

namespace PSSR.Logic.WorkPackageSteps.Concrete
{
    public class PlaceWorkPackageStepAction : BskaActionStatus, IPlaceWorkStepPackageAction
    {
        private readonly IPlaceWorkPackageStepDbAccess _dbAccess;
        private readonly IUpdateRoadMapDbAccess _workPackageDbAcces;

        public PlaceWorkPackageStepAction(IPlaceWorkPackageStepDbAccess dbAccess,IUpdateRoadMapDbAccess wokDbAccess)
        {
            _dbAccess = dbAccess;
            _workPackageDbAcces = wokDbAccess;
        }

        public WorkPackageStep BizAction(WorkPackageStepDto inputData)
        {
            var workPackage = _workPackageDbAcces.GetRoadMap(inputData.WorkPackageId);
            if(workPackage==null)
            {
                AddError("WorkPackage Not Valid...", "workPackage");
                return null;
            }
            var desStatus = WorkPackageStep.CreateWorkPackageStep(inputData.Title,inputData.Description,inputData.WorkPackageId);

            CombineErrors(desStatus);

            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
