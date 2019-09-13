using BskaGenericCoreLib;
using PSSR.DbAccess.ProjectSystems;

namespace PSSR.Logic.ProjectSystmes.Concrete
{
    public class UpdateSystemAction : BskaActionStatus, IUpdateSystemAction
    {
        private readonly IUpdateSystemDbAccess _dbAccess;
        public UpdateSystemAction(IUpdateSystemDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(ProjectSystemDto inputData)
        {
            var pSystem = _dbAccess.GetSystme(inputData.Id);
            if (pSystem == null)
            {
                AddError("Could not find the system. Someone entering illegal ids?");
                return;
            }
               

            var status = pSystem.UpdateProjectSystem(inputData.Code, inputData.Description);

            CombineErrors(status);

            Message = $"system is update: {pSystem.ToString()}.";
        }
    }
}
