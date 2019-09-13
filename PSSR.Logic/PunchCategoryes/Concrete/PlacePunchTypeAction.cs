using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DbAccess.PunchTypes;

namespace PSSR.Logic.PunchTypes.Concrete
{
    public class PlacePunchTypeAction : BskaActionStatus, IPlacePunchTypeAction
    {
        private readonly IPlacePunchTypeDbAccess _dbAccess;
        public PlacePunchTypeAction(IPlacePunchTypeDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public PunchType BizAction(PunchTypeDto inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData.Name))
            {
                AddError("Punch Type Name is Required.");
                return null;
            }

            if(inputData.WorkPackagepr==null)
            {
                AddError("Please insert value for workpackages percentage");
                return null;
            }

            var desStatus = PunchType.CreatePunchType(inputData.Name,inputData.ProjectId,inputData.WorkPackagepr);

            CombineErrors(desStatus);

            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
