using BskaGenericCoreLib;
using PSSR.DbAccess.RoadMaps;
using PSSR.DbAccess.WorkPackages;

namespace PSSR.Logic.WorkPackages.Concrete
{
    public class DeleteWorkPackageAction : BskaActionStatus, IDeleteWorkPackageAction
    {
        private readonly IDeleteWorkPackageDbAccess _dbAccess;
        private readonly IUpdateRoadMapDbAccess _updatedbAccess;
        public DeleteWorkPackageAction(IDeleteWorkPackageDbAccess dbAccess
            , IUpdateRoadMapDbAccess updatedbAccess)
        {
            _dbAccess = dbAccess;
            _updatedbAccess = updatedbAccess;
        }

        public void BizAction(int inputData)
        {
            var item = _updatedbAccess.GetRoadMap(inputData);
            if (item == null)
            {
                AddError("Could not find the projectwbs. Someone entering illegal ids?");
                return;
            }

            if (_updatedbAccess.HaveAnyActivity(item.Id))
            {
                AddError("Work Package hvae any activity!!!");
                return;
            }

            _dbAccess.Delete(item);

            Message = $"work package is Delete: {item.ToString()}.";
        }
    }
}
