
namespace PSSR.Logic.Punches.Concrete
{
    public class UpdatePunchAction : BskaGenericCoreLib.BskaActionStatus, IUpdatePunchDbAccess
    {
        private readonly DbAccess.Punchs.IUpadePunchDbAccess _dbAccess;

        public UpdatePunchAction(DbAccess.Punchs.IUpadePunchDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(PunchDto inputData)
        {
            var punch = _dbAccess.GetPunch(inputData.Id);
            if (punch == null)
            {
                AddError("Could not find the punch. Someone entering illegal ids?");
                return;
            }

            var status = punch.UpdatePunch( inputData.DefectDescription, inputData.OrginatedBy, inputData.CreatedBy, inputData.CheckBy
                , inputData.ApproveBy, inputData.OrginatedDate, inputData.CheckDate, inputData.ClearDate, inputData.EstimateMh, inputData.ActualMh,
                inputData.VendorRequired, inputData.VendorName, inputData.MaterialRequired, inputData.EnginerigRequired, inputData.ClearPlan, inputData.CorectiveAction
               );

            CombineErrors(status);

            Message = $"punch is update: {punch.ToString()}.";
        }
    }
}
