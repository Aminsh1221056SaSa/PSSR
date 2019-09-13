using BskaGenericCoreLib;
using PSSR.DbAccess.LocationTypes;

namespace PSSR.Logic.LocationTypes.Concrete
{
    public class LocationDeleteAction : BskaActionStatus, ILocationDeleteAction
    {
        private readonly IDeleteLocationDbAccess _dbAccess;
        private readonly IUpdateLocationTypeDbAccess _updatedbAccess;
        public LocationDeleteAction(IDeleteLocationDbAccess dbAccess
            , IUpdateLocationTypeDbAccess updatedbAccess)
        {
            _dbAccess = dbAccess;
            _updatedbAccess = updatedbAccess;
        }

        public void BizAction(int inputData)
        {
            var item = _updatedbAccess.GetLocationType(inputData);
            if (item == null)
                AddError("Could not find the location. Someone entering illegal ids?");

            if (_updatedbAccess.HaveAnyActivity(item.Id))
                AddError("Location hvae any activity!!!");

            _dbAccess.Delete(item);

            Message = $"location is Delete: {item.ToString()}.";
        }
    }
}
