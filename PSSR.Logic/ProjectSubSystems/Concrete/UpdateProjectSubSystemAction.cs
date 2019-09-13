using BskaGenericCoreLib;
using PSSR.DbAccess.ProjectSubSystems;

namespace PSSR.Logic.ProjectSubSystems.Concrete
{
    public class UpdateProjectSubSystemAction : BskaActionStatus, IUpdateSubSystemAction
    {
        private readonly IUpdateProjectSubSystemDbAccess _dbAccess;
        public UpdateProjectSubSystemAction(IUpdateProjectSubSystemDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(ProjectSubSystemDto inputData)
        {
            var pSystem = _dbAccess.GetSubSystme(inputData.Id);
            if (pSystem == null)
            {
                AddError("Could not find the Subsystem. Someone entering illegal ids?");
                return;
            }
           

            var status = pSystem.UpdateProjectSubSystem(inputData.Code, inputData.Description,
                inputData.PriorityNo,inputData.SubPriorityNo);

            CombineErrors(status);

            Message = $"Subsystem is update: {pSystem.ToString()}.";
        }
    }
}
