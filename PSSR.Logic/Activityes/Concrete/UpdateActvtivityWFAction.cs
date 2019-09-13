using BskaGenericCoreLib;
using PSSR.DbAccess.Activityes;
using System.Data;

namespace PSSR.Logic.Activityes.Concrete
{
    public class UpdateActvtivityWFAction : BskaActionStatus, IUpdateActivityWF
    {
        private readonly IUpdateActivityDbAccess _dbAccess;
        public UpdateActvtivityWFAction(IUpdateActivityDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(DataTable inputData)
        {
            _dbAccess.UpdateActivityWf(inputData);
        }
    }
}
