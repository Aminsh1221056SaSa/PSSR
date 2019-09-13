using BskaGenericCoreLib;
using PSSR.DbAccess.PunchCategoryies;
using System;

namespace PSSR.Logic.PunchCategoryes.Concrete
{
    public class DeletePunchTypeAction : BskaActionStatus, IDeletePunchCategoryAction
    {
        private readonly IDeletePunchCategoryDbAccess _dbAccess;
        private readonly IUpdatePunchCategoryDbAccess _updateDbAccess;
        public DeletePunchTypeAction(IDeletePunchCategoryDbAccess dbAccess
            , IUpdatePunchCategoryDbAccess updateDbAccess)
        {
            _dbAccess = dbAccess;
            _updateDbAccess = updateDbAccess;
        }

        public void BizAction(int inputData)
        {
            var item = _updateDbAccess.GetPunchCategory(inputData);
            if (item == null)
                throw new NullReferenceException("Could not find the punchCategory. Someone entering illegal ids?");

            if (_updateDbAccess.HaveAnyPunch(inputData))
                throw new NullReferenceException("punch Category have any punch and not allowed for delete!!!");

            _dbAccess.Delete(item);

            Message = $"Activity is Delete: {item.ToString()}.";
        }
    }
}
