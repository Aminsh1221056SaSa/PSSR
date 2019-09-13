using BskaGenericCoreLib;
using PSSR.DbAccess.Activityes;

namespace PSSR.Logic.Activityes.Concrete
{
    public class PlaceActivityDocumentAction : BskaActionStatus, IPlaceActivityDocumentAction
    {
        private readonly IUpdateActivityDbAccess _dbAccess;
        private readonly DbAccess.Punchs.IUpadePunchDbAccess _punchDbAccess;
        public PlaceActivityDocumentAction(IUpdateActivityDbAccess dbAccess
            , DbAccess.Punchs.IUpadePunchDbAccess punchdbAccess)
        {
            _dbAccess = dbAccess;
            _punchDbAccess = punchdbAccess;
        }

        public void BizAction(ActivityDocumentDto inputData)
        {
            var activity = _dbAccess.GetActivity(inputData.ActivityId);
            if (activity == null)
            {
                AddError("Could not find the Activity. Someone entering illegal ids?");
                return;
            }
              

            if(inputData.PunchId.HasValue)
            {
                var punch = _punchDbAccess.GetPunch(inputData.PunchId.Value);
                if (punch == null)
                {
                    AddError("Could not find the punch. Someone entering illegal ids?");
                    return;
                }
            }

            var status = activity.CreateActivityDocument(inputData.Description, inputData.FilePath, 
                inputData.ActivityId, inputData.PunchId);

            CombineErrors(status);

            Message = $"Activity Document is Added: {activity.ToString()}.";
        }
    }
}
