using BskaGenericCoreLib;
using PSSR.DbAccess.RoadMaps.Concrete;
using PSSR.DataLayer.EfClasses.Management;

namespace PSSR.Logic.RoadMaps.Concrete
{
    public class PlaceWorkPackageAction : BskaActionStatus, IPlaceWorkPackageAction
    {
        private readonly IPlaceWorkPackageDbAccess _dbAccess;
        public PlaceWorkPackageAction(IPlaceWorkPackageDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public WorkPackage BizAction(ProjectWorkPackageListDto inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData.Name))
            {
                AddError("Name is Required.");
                return null;
            }

            var desStatus = WorkPackage.CreateRoadMap(inputData.Name);

            CombineErrors(desStatus);

            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
