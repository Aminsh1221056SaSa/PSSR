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
                throw new NullReferenceException("Could not find the project. Someone entering illegal ids?");

            var status = project.UpdateProject(inputData.Description,inputData.StartDate,inputData.EndDate);

            CombineErrors(status);

            Message = $"project is update: {project.ToString()}.";
        }

    }
}
