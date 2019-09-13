using BskaGenericCoreLib;
using PSSR.DbAccess.WorkPackageSteps;
using System;

namespace PSSR.Logic.WorkPackageSteps.Concrete
{
    public class UpdateWorkPackageStepDbAccess : BskaActionStatus, IUpdateWorkPackageStepAction
    {
        private readonly IUpdateWorkPackageStepDbAccess _dbAccess;
        public UpdateWorkPackageStepDbAccess(IUpdateWorkPackageStepDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(WorkPackageStepDto inputData)
        {
            var roadMap = _dbAccess.GetWorkPackageStep(inputData.Id);
            if (roadMap == null)
                throw new NullReferenceException("Could not find the workPackage step. Someone entering illegal ids?");

            var status = roadMap.UpdateWorkPackageStep(inputData.Title,inputData.Description);

            CombineErrors(status);

            Message = $"work package is update: {roadMap.ToString()}.";
        }
    }
}
