using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DbAccess.Projects;

namespace PSSR.Logic.Projects.Concrete
{
    public class PlaceProjectAction : BskaActionStatus, IPlaceProjectAction
    {
        private readonly IPlaceProjectDbAccess _dbAccess;
        public PlaceProjectAction(IPlaceProjectDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public Project BizAction(ProjectDto inputData)
        {
            var desStatus = Project.CreateProject(inputData.Description,inputData.ContractorId
                ,inputData.StartDate,inputData.EndDate,inputData.Type);

            CombineErrors(desStatus);

            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
