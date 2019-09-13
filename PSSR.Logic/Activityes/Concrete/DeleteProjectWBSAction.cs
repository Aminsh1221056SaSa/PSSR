using BskaGenericCoreLib;
using PSSR.Common;
using PSSR.DbAccess.Activityes;
using System;

namespace PSSR.Logic.Activityes.Concrete
{
    public class DeleteProjectWBSAction : BskaActionStatus, IDeleteActivityAction
    {
        private readonly IDeleteActivityDbAccess _dbAccess;
        private readonly IUpdateActivityDbAccess _updateDbAccess;
        public DeleteProjectWBSAction(IDeleteActivityDbAccess dbAccess
            , IUpdateActivityDbAccess updateDbAccess)
        {
            _dbAccess = dbAccess;
            _updateDbAccess = updateDbAccess;
        }

        public void BizAction(long inputData)
        {
            var item = _updateDbAccess.GetActivity(inputData);
            if (item == null)
                throw new NullReferenceException("Could not find the activity. Someone entering illegal ids?");

            if (item.Status !=ActivityStatus.NotStarted)
                throw new NullReferenceException("Activity not allowed for delete!!!");

            _dbAccess.Delete(item);

            Message = $"Activity is Delete: {item.ToString()}.";
        }
    }
}
