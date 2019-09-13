using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DbAccess.Projects;

namespace PSSR.Logic.Projects.Concrete
{
    public class PlaceProjectWBSAction : BskaActionStatus, IPlaceProjectWBSAction
    {
        private readonly IPlaceProjectWBSDbAccess _dbAccess;
        public PlaceProjectWBSAction(IPlaceProjectWBSDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public ProjectWBS BizAction(ProjectWBSDto inputData)
        {
            var desStatus = ProjectWBS.CreateProjectWBS(inputData.Type,inputData.TargetId,inputData.WF,inputData.WBSCode
                ,inputData.ProjectId,inputData.ParentId,inputData.Name,inputData.CalculationType);

            CombineErrors(desStatus);

            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
