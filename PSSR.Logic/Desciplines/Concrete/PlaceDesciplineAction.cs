using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DbAccess.Desciplines;

namespace PSSR.Logic.Desciplines.Concrete
{
    public class PlaceDesciplineAction : BskaActionStatus, IPlaceDesciplineAction
    {
        private readonly IPlaceDesciplineDbAccess _dbAccess;
        public PlaceDesciplineAction(IPlaceDesciplineDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public Descipline BizAction(PlaceDesciplineDto inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData.Descipline.Name))
            {
                AddError("Descipline Name is Required.");
                return null;
            }

            var desStatus = Descipline.CreateDesciplineFactory(
             inputData.Descipline.Name,inputData.Descipline.Description,inputData.Descipline.ProjectIds);

            CombineErrors(desStatus);

            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
