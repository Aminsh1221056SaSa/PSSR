using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DbAccess.ProjectSubSystems;
using System.Collections.Generic;
using System.Linq;

namespace PSSR.Logic.ProjectSubSystems.Concrete
{
    public class PlcaeSubSystemBulkAction : BskaActionStatus, IPlcaeSubSystemBulkAction
    {
        private readonly IPlaceProjectSubSystemDbAccess _dbAccess;
        public PlcaeSubSystemBulkAction(IPlaceProjectSubSystemDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }


        public void BizAction(List<ProjectSubSystem> inputData)
        {
            if (!inputData.Any())
            {
                AddError("system Code is Required.");
            }

            _dbAccess.AddBulck(inputData);
        }
    }
}
