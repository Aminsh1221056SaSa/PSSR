using BskaGenericCoreLib;
using PSSR.DbAccess.LocationTypes;
using System;

namespace PSSR.Logic.LocationTypes.Concrete
{
    public class UpdateLocationTypeAction : BskaActionStatus, IUpdateLocationTypeAction
    {
        private readonly IUpdateLocationTypeDbAccess _dbAccess;

        public UpdateLocationTypeAction(IUpdateLocationTypeDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(LocationTypeDto inputData)
        {
            var location = _dbAccess.GetLocationType(inputData.Id);
            if (location == null)
                throw new NullReferenceException("Could not find the location. Someone entering illegal ids?");

            var status = location.UpdateLocationType(inputData.Title);

            CombineErrors(status);

            Message = $"person is update: {location.ToString()}.";
        }
    }
}
