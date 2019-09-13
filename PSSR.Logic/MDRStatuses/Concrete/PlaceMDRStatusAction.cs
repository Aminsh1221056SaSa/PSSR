using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects.MDRS;
using PSSR.DbAccess.MDRStatuses;

namespace PSSR.Logic.MDRStatuses.Concrete
{
    public class PlaceMDRStatusAction : BskaActionStatus, IPlaceMDRStatusAction
    {
        private readonly IPlaceMDRStatusDbAccess _dbAccess;

        public PlaceMDRStatusAction(IPlaceMDRStatusDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public MDRStatus BizAction(MDRStatusDto inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData.Name))
            {
                AddError("Name is Required.");
                return null;
            }

            var desStatus = MDRStatus.CreateMDRStatus(inputData.Name, inputData.Wf, inputData.ProjectId,inputData.Description);

            CombineErrors(desStatus);

            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
