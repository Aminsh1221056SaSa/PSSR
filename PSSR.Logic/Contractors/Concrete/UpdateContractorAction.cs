using BskaGenericCoreLib;
using PSSR.DbAccess.Contractors;
using System;

namespace PSSR.Logic.Contractors.Concrete
{
    public class UpdateContractorAction : BskaActionStatus, IUpdateContractorAction
    {
        private readonly IUpdateContractorDbAccess _dbAccess;

        public UpdateContractorAction(IUpdateContractorDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(ContractorDto inputData)
        {
            var contractor = _dbAccess.GetContractor(inputData.Id);
            if (contractor == null)
                throw new NullReferenceException("Could not find the contractor. Someone entering illegal ids?");

            var status = contractor.UpdateContractor(inputData.Name, inputData.PhoneNumber, inputData.Address, inputData.ContractDate);
            CombineErrors(status);

            Message = $"contractor is update: {contractor.ToString()}.";
        }
    }
}
