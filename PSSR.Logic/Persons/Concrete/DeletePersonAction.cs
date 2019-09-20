using BskaGenericCoreLib;
using PSSR.DbAccess.Persons;

namespace PSSR.Logic.Persons.Concrete
{
    public class DeleteContractorAction : BskaActionStatus, IDeletePersonAction
    {
        private readonly IDeletePersonDbAccess _dbAccess;
        private readonly IUpdatePersonDbAccess _updatedbAccess;
        public DeleteContractorAction(IDeletePersonDbAccess dbAccess
            , IUpdatePersonDbAccess updatedbAccess)
        {
            _dbAccess = dbAccess;
            _updatedbAccess = updatedbAccess;
        }

        public void BizAction(int inputData)
        {
            var item = _updatedbAccess.GetPerson(inputData);
            if (item == null)
                AddError("Could not find the person. Someone entering illegal ids?");

            if (_updatedbAccess.HaveAnyPorjects(item.Id))
                AddError("person hvae some projects!!!");

            _dbAccess.Delete(item);

            Message = $"person is Delete: {item.ToString()}.";
        }
    }
}
