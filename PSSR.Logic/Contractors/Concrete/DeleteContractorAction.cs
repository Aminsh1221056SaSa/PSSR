using BskaGenericCoreLib;
using PSSR.DbAccess.Contractors;

namespace PSSR.Logic.Contractors.Concrete
{
    public class DeleteContractorAction : BskaActionStatus, IDeleteContractorAction
    {
        private readonly IDeleteContractorDbAccess _dbAccess;
        private readonly IUpdateContractorDbAccess _updatedbAccess;
        public DeleteContractorAction(IDeleteContractorDbAccess dbAccess
            , IUpdateContractorDbAccess updatedbAccess)
        {
            _dbAccess = dbAccess;
            _updatedbAccess = updatedbAccess;
        }

        public void BizAction(int inputData)
        {
            var item = _updatedbAccess.GetContractor(inputData);
            if (item == null)
                AddError("Could not find the contractor. Someone entering illegal ids?");

            if (_updatedbAccess.HaveAnyPorjects(item.Id))
                AddError("Contractor hvae some projects!!!");

            _dbAccess.Delete(item);

            Message = $"Contractor is Delete: {item.ToString()}.";
        }
    }
}
