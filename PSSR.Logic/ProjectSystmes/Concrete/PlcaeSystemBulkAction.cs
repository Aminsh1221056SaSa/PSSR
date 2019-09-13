using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DbAccess.ProjectSystems;
using System.Collections.Generic;
using System.Linq;

namespace PSSR.Logic.ProjectSystmes.Concrete
{
    public class PlcaeSystemBulkAction : BskaActionStatus, IPlcaeSystemBulkAction
    {
        private readonly IPlaceSystemDbAccess _dbAccess;
        public PlcaeSystemBulkAction(IPlaceSystemDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(List<ProjectSystem> inputData)
        {
            if (!inputData.Any())
            {
                AddError("system Code is Required.");
            }

            _dbAccess.AddBulck(inputData);
        }
    }
}
