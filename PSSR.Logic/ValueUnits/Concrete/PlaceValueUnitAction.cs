using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DbAccess.ValueUnits;

namespace PSSR.Logic.ValueUnits.Concrete
{
    public class PlaceValueUnitAction : BskaActionStatus, IPlaceValuUnitAction
    {
        private readonly IPlaceValueUnitDbAccess _dbAccess;
        public PlaceValueUnitAction(IPlaceValueUnitDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public ValueUnit BizAction(ValueUnitDto inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData.Name))
            {
                AddError("Value Unit Name is Required.");
                return null;
            }

            if (inputData.ParentId.HasValue)
            {
                if (!_dbAccess.HaveValidParent(inputData.ParentId.Value))
                {
                    AddError("Value unit parnet is not valid.");
                    return null;
                }
            }

            var desStatus = ValueUnit.CreateValueUnit(inputData.Name, inputData.MathType,inputData.MathNum,inputData.ParentId);

            CombineErrors(desStatus);

            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
