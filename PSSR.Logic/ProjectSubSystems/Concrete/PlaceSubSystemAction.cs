using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DbAccess.ProjectSubSystems;

namespace PSSR.Logic.ProjectSubSystems.Concrete
{
    public class PlaceSubSystemAction : BskaActionStatus, IPlaceSubSystemAction
    {
        private readonly IPlaceProjectSubSystemDbAccess _dbAccess;
        public PlaceSubSystemAction(IPlaceProjectSubSystemDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public ProjectSubSystem BizAction(ProjectSubSystemDto inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData.Code))
            {
                AddError("Code is Required.");
                return null;
            }

            var desStatus = ProjectSubSystem.CreateProjectSubSystem(inputData.Code,inputData.Description,inputData.ProjectSystemId
                ,inputData.PriorityNo,inputData.SubPriorityNo);

            CombineErrors(desStatus);

            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
