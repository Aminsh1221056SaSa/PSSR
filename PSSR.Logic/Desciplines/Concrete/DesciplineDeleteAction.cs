using BskaGenericCoreLib;
using PSSR.DbAccess.Desciplines;

namespace PSSR.Logic.Desciplines.Concrete
{
    public class DesciplineDeleteAction : BskaActionStatus, IDesciplineDeleteAction
    {
        private readonly IDeleteDesciplineDbAccess _dbAccess;
        private readonly IUpdateDesciplineDbAccess _updatedbAccess;
        public DesciplineDeleteAction(IDeleteDesciplineDbAccess dbAccess
            , IUpdateDesciplineDbAccess updatedbAccess)
        {
            _dbAccess = dbAccess;
            _updatedbAccess = updatedbAccess;
        }

        public void BizAction(int inputData)
        {
            var item = _updatedbAccess.GetDescipline(inputData);
            if (item == null)
                AddError("Could not find the descipline. Someone entering illegal ids?");

            _dbAccess.Delete(item);

            Message = $"Descipline is Delete: {item.ToString()}.";
        }
    }
}
