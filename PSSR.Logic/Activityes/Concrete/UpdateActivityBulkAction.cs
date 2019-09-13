using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DbAccess.Activityes;
using System.Collections.Generic;

namespace PSSR.Logic.Activityes.Concrete
{
    public class UpdateActivityBulkAction : BskaActionStatus, IUpdateActivityBulkAction
    {
        private readonly IUpdateActivityDbAccess _dbAccess;
        public UpdateActivityBulkAction(IUpdateActivityDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(List<Activity> inputData)
        {
            _dbAccess.UpdateBulck(inputData);
        }
    }
}
