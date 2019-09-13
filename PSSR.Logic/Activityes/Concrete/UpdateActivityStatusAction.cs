using BskaGenericCoreLib;
using PSSR.DbAccess.Activityes;

namespace PSSR.Logic.Activityes.Concrete
{
    public class UpdateActivityStatusAction : BskaActionStatus, IUpdateActivityStatusAction
    {
        private readonly IUpdateActivityDbAccess _dbAccess;
        public UpdateActivityStatusAction(IUpdateActivityDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(ActivityStatusUpdateDto inputData)
        {
            var activity = _dbAccess.GetActivity(inputData.Id);
            if (activity == null)
            {
                AddError("Could not find the Activity. Someone entering illegal ids?");
                return;
            }


            var status = activity.UpdateActivityStatus(inputData.Status, inputData.HoldBy,
                inputData.Condition);
            CombineErrors(status);

            Message = $"activity status is update: {activity.ToString()}.";
        }
    }
}
