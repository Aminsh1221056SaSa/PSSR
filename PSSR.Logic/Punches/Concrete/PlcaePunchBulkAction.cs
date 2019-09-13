using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DbAccess.Activityes;
using PSSR.DbAccess.Punchs;
using System.Collections.Generic;
using System.Linq;

namespace PSSR.Logic.Punches.Concrete
{
    public class PlcaePunchBulkAction : BskaActionStatus, IPlcaePunchBulkAction
    {
        private readonly IPlacePunchDbAccess _dbAccess;
        private readonly IUpdateActivityDbAccess _activityDbAccess;
        public PlcaePunchBulkAction(IPlacePunchDbAccess dbAccess,IUpdateActivityDbAccess acDbAccess)
        {
            _dbAccess = dbAccess;
            _activityDbAccess = acDbAccess;
        }

        public void BizAction(List<Punch> inputData)
        {
            if (!inputData.Any())
            {
                AddError("punch list is Required.");
                return;
            }

            var acCods = inputData.Select(s => s.ActivityId).ToList();
            var activityes = _activityDbAccess.GetActivityByActivityId(acCods);
            if(activityes.Any(s=>s.Status!=Common.ActivityStatus.Done))
            {
                var notDone= activityes.Where(s => s.Status != Common.ActivityStatus.Done)
                .Select(bu => bu.ActivityCode).AsEnumerable().Aggregate("", (ac, bc) => ac + "," + bc);
                AddError($"Current Tasks isn't in Done Status --{notDone}");
                return;
            }

            _dbAccess.AddBulck(inputData);
        }
    }
}
