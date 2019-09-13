using BskaGenericCoreLib;
using PSSR.DbAccess.Projects;

namespace PSSR.Logic.Projects.Concrete
{
    public class DeleteProjectWBSAction : BskaActionStatus, IDeleteProjectWBSAction
    {
        private readonly IDeleteProjectWBSDbAccess _dbAccess;
        private readonly IUpdateProjectWBSDbAccess _updateDbAccess;
        public DeleteProjectWBSAction(IDeleteProjectWBSDbAccess dbAccess
            ,IUpdateProjectWBSDbAccess updateDbAccess)
        {
            _dbAccess = dbAccess;
            _updateDbAccess = updateDbAccess;
        }

        public void BizAction(long inputData)
        {
            var item = _updateDbAccess.GetProject(inputData);
            if (item == null)
                AddError("Could not find the projectwbs. Someone entering illegal ids?");

            if (item.Type == Common.WBSType.Project)
                AddError("Project wbs node not allowed for delete!!!");

            if (item.Type == Common.WBSType.Activity)
                AddError("MDR wbs node not allowed for delete!!!");

            _dbAccess.Delete(item);
            
            Message = $"projectwbs is Delete: {item.ToString()}.";
        }
    }
}
