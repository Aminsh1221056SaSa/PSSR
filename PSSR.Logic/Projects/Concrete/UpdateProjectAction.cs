using BskaGenericCoreLib;
using PSSR.DbAccess.Projects;
using System;

namespace PSSR.Logic.Projects.Concrete
{
    public class UpdateProjectAction : BskaActionStatus, IUpdateProjectAction
    {
        private readonly IUpdateProjectDbAccess _dbAccess;
        public UpdateProjectAction(IUpdateProjectDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(ProjectDto inputData)
        {
            var project = _dbAccess.GetProject(inputData.Id);
            if (project == null)
            {
                AddError("Could not find the project. Someone entering illegal ids?");
            }
            if (_dbAccess.haveAnyWbs(inputData.Id))
            {
                AddError("Project have some wbs items and could not to edit.");
            }
            if (!HasErrors)
            {
                var status = project.UpdateProject(inputData.Description, inputData.ContractorId, inputData.StartDate, inputData.EndDate, inputData.Type);

                CombineErrors(status);

                Message = $"project is update: {project.ToString()}.";
            }
        }
    }
}
