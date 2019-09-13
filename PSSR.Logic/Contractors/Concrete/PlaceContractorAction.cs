using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Person;
using PSSR.DbAccess.Contractors;

namespace PSSR.Logic.Contractors.Concrete
{
    public class PlaceContractorAction : BskaActionStatus, IPlaceContractorAction
    {
        private readonly IPlaceContractorDbAccess _dbAccess;
        public PlaceContractorAction(IPlaceContractorDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public Contractor BizAction(ContractorDto inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData.Name))
            {
                AddError("Contractor Name is Required.");
                return null;
            }

            var desStatus = Contractor.CreateContractor(inputData.Name,inputData.PhoneNumber,inputData.Address,inputData.ContractDate);
            CombineErrors(desStatus);


            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
