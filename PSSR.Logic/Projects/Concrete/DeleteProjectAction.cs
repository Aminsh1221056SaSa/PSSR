using BskaGenericCoreLib;
using PSSR.DbAccess.Projects;
using System;

namespace PSSR.Logic.Projects.Concrete
{
    public class DeleteProjectAction : BskaActionStatus, IDeleteProjectAction
    {
        private readonly IDeleteProjectDbAccess _dbAccess;
        private readonly IUpdateProjectDbAccess _updatedbAccess;
        public DeleteProjectAction(IDeleteProjectDbAccess dbAccess
            , IUpdateProjectDbAccess updatedbAccess)
        {
            _dbAccess = dbAccess;
            _updatedbAccess = updatedbAccess;
        }

        public void BizAction(Guid inputData)
        {
            var item = _updatedbAccess.GetProject(inputData);
            if (item == null)
                AddError("Could not find the Project. Someone entering illegal ids?");

            if (_updatedbAccess.haveAnyWbs(item.Id))
                AddError("Project hvae some wbs items!!!");

            _dbAccess.Delete(item);

            Message = $"Project is Delete: {item.ToString()}.";
        }
    }
}
