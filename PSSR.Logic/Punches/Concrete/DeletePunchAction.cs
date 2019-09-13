using BskaGenericCoreLib;
using PSSR.DbAccess.Punchs;

namespace PSSR.Logic.Punches.Concrete
{
    public class DeletePunchAction : BskaActionStatus, IDeletePunchAction
    {
        private readonly IDeletePunchDbAccess _dbAccess;
        private readonly IUpadePunchDbAccess _updatedbAccess;

        public DeletePunchAction(IDeletePunchDbAccess dbAccess, IUpadePunchDbAccess updatedbAccess)
        {
            _dbAccess = dbAccess;
            _updatedbAccess = updatedbAccess;
        }

        public void BizAction(long inputData)
        {
            var item = _updatedbAccess.GetPunch(inputData);
            if (item == null)
            {
                AddError("Could not find the punch. Someone entering illegal ids?");
                return;
            }

            if (item.ClearDate.HasValue)
            {
                AddError("punch is clear and not alowed for delete!!!");
                return;
            }

            _dbAccess.Delete(item);

            Message = $"work package is Delete: {item.ToString()}.";
        }
    }
}
