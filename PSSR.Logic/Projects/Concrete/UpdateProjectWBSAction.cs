using BskaGenericCoreLib;
using PSSR.DbAccess.Projects;
using System;

namespace PSSR.Logic.Projects.Concrete
{
    public class UpdateProjectWBSAction : BskaActionStatus, IUpdateProjectWBSAction
    {
        private readonly IUpdateProjectWBSDbAccess _dbAccess;
        public UpdateProjectWBSAction(IUpdateProjectWBSDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(ProjectWBSDto inputData)
        {
            var projectwbs = _dbAccess.GetProject(inputData.Id);
            if (projectwbs == null)
                throw new NullReferenceException("Could not find the projectwbs. Someone entering illegal ids?");

            var status = projectwbs.UpdateProject(inputData.WBSCode,inputData.CalculationType);

            CombineErrors(status);

            Message = $"projectwbs is update: {projectwbs.ToString()}.";
        }
    }
}
