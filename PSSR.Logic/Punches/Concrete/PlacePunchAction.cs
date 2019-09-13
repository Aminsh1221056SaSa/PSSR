
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DbAccess.Activityes;
using PSSR.DbAccess.PunchTypes;

namespace PSSR.Logic.Punches.Concrete
{
    public class PlacePunchAction : BskaGenericCoreLib.BskaActionStatus, IPlacePunchAction
    {
        private readonly DbAccess.Punchs.IPlacePunchDbAccess _dbAccess;
        private readonly IUpdateActivityDbAccess _activityDbAccess;
        private readonly IUpdatePunchTypeDbAccess _dbAccessPunchType;

        public PlacePunchAction(DbAccess.Punchs.IPlacePunchDbAccess dbAccess,IUpdateActivityDbAccess acDbAcces
            , IUpdatePunchTypeDbAccess punchTypeDb)
        {
            _dbAccess = dbAccess;
            _activityDbAccess = acDbAcces;
            _dbAccessPunchType = punchTypeDb;
        }

        public Punch BizAction(PunchDto inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData.Code))
            {
                AddError("Code is Required.");
                return null;
            }

            var activity = _activityDbAccess.GetActivityToPunches(inputData.ActivityId);

            if (activity == null)
            {
                AddError("activity not valid!!!", "Activity");
                return null;
            }

            if (activity.Status != Common.ActivityStatus.Done)
            {
                AddError("Activity is not completed and don't allow for add punch!!!", "Activity");
                return null;
            }

            var punchType = _dbAccessPunchType.GetPunchTypeToWorkPackages(inputData.PunchTypeId);
            if(punchType==null)
            {
                AddError("Invalied punch Type!!!", "Activity");
                return null;
            }

            var desStatus = Punch.CreatePunch(inputData.Code,inputData.DefectDescription,inputData.OrginatedBy,inputData.CreatedBy,inputData.CheckBy
                ,inputData.ApproveBy,inputData.OrginatedDate,inputData.CheckDate,inputData.ClearDate,inputData.EstimateMh,inputData.ActualMh,
                inputData.VendorRequired,inputData.VendorName,inputData.MaterialRequired,inputData.EnginerigRequired,inputData.ClearPlan,inputData.CorectiveAction
                ,inputData.PunchTypeId,inputData.ActivityId,inputData.CategoryId);

            CombineErrors(desStatus);

            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
