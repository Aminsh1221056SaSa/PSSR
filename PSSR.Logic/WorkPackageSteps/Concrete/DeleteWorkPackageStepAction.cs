using BskaGenericCoreLib;
using PSSR.DbAccess.WorkPackageSteps;

namespace PSSR.Logic.WorkPackageSteps.Concrete
{
    public class DeleteWorkPackageStepAction : BskaActionStatus, IDeleteWorkPackageStepAction
    {
        private readonly IDeleteWorkPackageStepDbAccess _dbAccess;
        private readonly IUpdateWorkPackageStepDbAccess _updatedbAccess;

        public DeleteWorkPackageStepAction(IDeleteWorkPackageStepDbAccess dbAccess
            , IUpdateWorkPackageStepDbAccess updatedbAccess)
        {
            _dbAccess = dbAccess;
            _updatedbAccess = updatedbAccess;
        }

        public void BizAction(int inputData)
        {
            var item = _updatedbAccess.GetWorkPackageStep(inputData);
            if (item == null)
            {
                AddError("Could not find the workpackage step. Someone entering illegal ids?");
                return;
            }

            if (_updatedbAccess.HaveAnyActivity(item.Id))
            {
                AddError("Work Package step hvae any activity!!!");
                return;
            }

            _dbAccess.Delete(item);

            Message = $"work package step is Delete: {item.ToString()}.";
        }
    }
}
