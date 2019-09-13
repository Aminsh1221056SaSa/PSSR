using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DbAccess.Punchs;
using System.Collections.Generic;

namespace PSSR.Logic.Punches.Concrete
{
    public class UpdatePunchBulkAction : BskaActionStatus, IUpdatePunchBulkAction
    {
        private readonly IUpadePunchDbAccess _dbAccess;
        public UpdatePunchBulkAction(IUpadePunchDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public void BizAction(List<Punch> inputData)
        {
            _dbAccess.UpdateBulck(inputData);
        }
    }
}
