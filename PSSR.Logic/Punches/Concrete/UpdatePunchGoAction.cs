using BskaGenericCoreLib;

namespace PSSR.Logic.Punches.Concrete
{
    public class UpdatePunchGoAction : BskaActionStatus, IUpdatePunchGoAction
    {
        private readonly DbAccess.Punchs.IUpadePunchDbAccess _dbAccess;

        public UpdatePunchGoAction(DbAccess.Punchs.IUpadePunchDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(PunchGoDto inputData)
        {
            var punch = _dbAccess.GetPunch(inputData.Id);
            if (punch == null)
            {
                AddError("Could not find the punch. Someone entering illegal ids?");
                return;
            }

            if(punch.CheckDate.HasValue)
            {
                AddError("punch is approved and do not allow for modify");
                return;
            }

            IStatusGeneric status = null;

            if(!punch.ClearDate.HasValue)
            {
               status= punch.UpdateClear(inputData.ClearBy, inputData.ClearDate);
            }
            else if(!punch.CheckDate.HasValue)
            {
                status = punch.UpdateApprove(inputData.CheckBy, inputData.ApproveBy, inputData.CheckDate);
            }

            CombineErrors(status);

            Message = $"punch is update: {punch.ToString()}.";
        }
    }
}
