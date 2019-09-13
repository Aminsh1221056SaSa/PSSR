using BskaGenericCoreLib;
using PSSR.DbAccess.PunchTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Logic.PunchTypes.Concrete
{
    public class UpdatePunchTypeAction : BskaActionStatus, IUpdatePunchTypeAction
    {
        private readonly IUpdatePunchTypeDbAccess _dbAccess;

        public UpdatePunchTypeAction(IUpdatePunchTypeDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }
        public void BizAction(PunchTypeDto inputData)
        {
            var formDictioanry = _dbAccess.GetPunchType(inputData.Id);
            if (formDictioanry == null)
                throw new NullReferenceException("Could not find the punch type. Someone entering illegal ids?");

            var status = formDictioanry.UpdatePunchType(inputData.Name);

            CombineErrors(status);

            Message = $"punch type is update: {formDictioanry.ToString()}.";
        }
    }
}
