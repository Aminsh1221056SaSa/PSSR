using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DbAccess.Activityes;
using System.Collections.Generic;

namespace PSSR.Logic.Activityes.Concrete
{
    public class PlcaeActivityBulkAction : BskaActionStatus, IPlcaeActivityBulkAction
    {
        private readonly IPlaceActivityDbAccess _dbAccess;
        public PlcaeActivityBulkAction(IPlaceActivityDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(List<Activity> inputData)
        {
            _dbAccess.AddBulck(inputData);
        }
    }
}
