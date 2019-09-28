using BskaGenericCoreLib;
using PSSR.DbAccess.ValueUnits;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Logic.ValueUnits.Concrete
{
    public class DeleteValueUnitAction : BskaActionStatus, IDeleteValueUnitAction
    {
        private readonly IDeleteValueUnitDbAccess _dbAccess;
        private readonly IUpdateValueUnitDbAccess _updatedbAccess;
        public DeleteValueUnitAction(IDeleteValueUnitDbAccess dbAccess
            , IUpdateValueUnitDbAccess updatedbAccess)
        {
            _dbAccess = dbAccess;
            _updatedbAccess = updatedbAccess;
        }

        public void BizAction(int inputData)
        {
            var item = _updatedbAccess.GetValueUnit(inputData);
            if (item == null)
                AddError("Could not find the value unit. Someone entering illegal ids?");

            _dbAccess.Delete(item);

            Message = $"value unit is Delete: {item.ToString()}.";
        }
    }
}
