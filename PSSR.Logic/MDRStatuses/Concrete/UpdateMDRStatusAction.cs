using BskaGenericCoreLib;
using PSSR.DbAccess.MDRStatuses;

namespace PSSR.Logic.MDRStatuses.Concrete
{
    public class UpdateMDRStatusAction : BskaActionStatus, IUpdateMDRStatusAction
    {
        private readonly IUpdateMDRStatusDbAccess _dbAccess;

        public UpdateMDRStatusAction(IUpdateMDRStatusDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }
        public void BizAction(MDRStatusDto inputData)
        {
            var MDRstatus = _dbAccess.GetMdrStatus(inputData.Id);
            if (MDRstatus == null)
            {
                AddError("Could not find the MDRStatus. Someone entering illegal ids?");
                return;
            }

            var status = MDRstatus.UpdateMDRStatus(inputData.Name,inputData.Wf,inputData.Description);

            CombineErrors(status);

            Message = $"MDR status is update: {MDRstatus.ToString()}.";
        }
    }
}
