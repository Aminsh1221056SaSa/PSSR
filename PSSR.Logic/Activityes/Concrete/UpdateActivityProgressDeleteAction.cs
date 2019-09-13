using BskaGenericCoreLib;
using PSSR.DbAccess.Activityes;
using PSSR.DbAccess.Punchs;
using PSSR.DbAccess.PunchTypes;
using System.Linq;

namespace PSSR.Logic.Activityes.Concrete
{
    public class UpdateActivityProgressDeleteAction : BskaActionStatus, IUpdateActivityProgressDeleteAction
    {
        private readonly IUpdateActivityDbAccess _activityDbAccess;
        private readonly IUpdatePunchTypeDbAccess _dbAccessPunchType;
        private readonly IUpadePunchDbAccess _updatedbAccess;

        public UpdateActivityProgressDeleteAction(IUpdateActivityDbAccess acDbAcces, IUpdatePunchTypeDbAccess punchTypeDb
            , IUpadePunchDbAccess updatedbAccess)
        {
            _activityDbAccess = acDbAcces;
            _dbAccessPunchType = punchTypeDb;
            _updatedbAccess = updatedbAccess;
        }

        public void BizAction(long inputData)
        {
            var punch = _updatedbAccess.GetPunch(inputData);
            if (punch == null)
            {
                AddError("Could not find the punch. Someone entering illegal ids?");
                return;
            }

            var activity = _activityDbAccess.GetActivityToPunches(punch.ActivityId);
            if (activity == null)
            {
                AddError("activity not valid!!!", "Activity");
                return;
            }

            var punchType = _dbAccessPunchType.GetPunchTypeToWorkPackages(punch.PunchTypeId);
            if (punchType == null)
            {
                AddError("Invalied punch Type!!!", "Activity");
                return;
            }

            var wokPackage = punchType.WorkPackages.FirstOrDefault(s => s.WorkPackageId == activity.WorkPackageId);
            if (wokPackage == null)
            {
                AddError("Punch type of WorkPackage is undefined!!!", "Activity");
                return;
            }
            float precentage = 0;

            if (activity.Punchs.Where(s => s.PunchTypeId == punch.PunchTypeId).Count()==1)
            {
                precentage = activity.Progress + wokPackage.Precentage;
                activity.UpdateActivityProgress(precentage);
            }
        }
    }
}
