using BskaGenericCoreLib;
using PSSR.DbAccess.ValueUnits;
using System;

namespace PSSR.Logic.ValueUnits.Concrete
{
    public class UpdateValueUnitAction : BskaActionStatus, IUpdateValueUnitAction
    {
        private readonly IUpdateValueUnitDbAccess _dbAccess;

        public UpdateValueUnitAction(IUpdateValueUnitDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(ValueUnitDto inputData)
        {
            var valueUnit = _dbAccess.GetValueUnit(inputData.Id);
            if (valueUnit == null)
                throw new NullReferenceException("Could not find the value unit. Someone entering illegal ids?");

            var status = valueUnit.UpdateValueUnit(inputData.Name, inputData.MathType, inputData.MathNum);

            CombineErrors(status);

            Message = $"value unit is update: {valueUnit.ToString()}.";
        }
    }
}
