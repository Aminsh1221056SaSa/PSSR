using BskaGenericCoreLib;
using PSSR.DbAccess.Projects;
using System;

namespace PSSR.Logic.Projects.Concrete
{
    public class UpdateProjectWBSWFAction : BskaActionStatus, IUpdateProjectWBSWFAction
    {
        private readonly IUpdateProjectWBSDbAccess _dbAccess;
        public UpdateProjectWBSWFAction(IUpdateProjectWBSDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(ProjectWBSDto inputData)
        {
            var projectwbs = _dbAccess.GetProject(inputData.Id);
            if (projectwbs == null)
                throw new NullReferenceException("Could not find the projectwbs. Someone entering illegal ids?");

            var status = projectwbs.UpdateProjectWF(inputData.WF);

            CombineErrors(status);

            Message = $"projectwbs wf is update: {projectwbs.ToString()}.";
        }
    }
}
