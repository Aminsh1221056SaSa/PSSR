using BskaGenericCoreLib;
using PSSR.DbAccess.PunchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.Logic.PunchTypes.Concrete
{
    public class DeletePunchTypeAction : BskaActionStatus, IDeletePunchTypeAction
    {
        private readonly IDeletePunchTypeDbAccess _dbAccess;
        private readonly IUpdatePunchTypeDbAccess _updateDbAccess;
        public DeletePunchTypeAction(IDeletePunchTypeDbAccess dbAccess
            , IUpdatePunchTypeDbAccess updateDbAccess)
        {
            _dbAccess = dbAccess;
            _updateDbAccess = updateDbAccess;
        }

        public void BizAction(int inputData)
        {
            var item = _updateDbAccess.GetPunchType(inputData);
            if (item == null)
                throw new NullReferenceException("Could not find the punchType. Someone entering illegal ids?");

            if (_updateDbAccess.HaveAnyPunch(inputData))
                throw new NullReferenceException("punch type have any punch and not allowed for delete!!!");

            _dbAccess.Delete(item);

            Message = $"Activity is Delete: {item.ToString()}.";
        }
    }
}
