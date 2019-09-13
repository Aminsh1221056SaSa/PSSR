using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DbAccess.LocationTypes;

namespace PSSR.Logic.LocationTypes.Concrete
{
    public class PlaceLocationTypeAction : BskaActionStatus, IPlaceLocationTypeAction
    {
        private readonly IPlaceLocationTypeDbAccess _dbAccess;

        public PlaceLocationTypeAction(IPlaceLocationTypeDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public LocationType BizAction(LocationTypeDto inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData.Title))
            {
                AddError("title is Required.");
                return null;
            }
            
            var desStatus = LocationType.CreateLocationType(inputData.Title);

            CombineErrors(desStatus);

            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
