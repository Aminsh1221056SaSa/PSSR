using System;
using System.Collections.Generic;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.WorkPackageSteps.Concrete
{
    public class DeleteWorkPackageStepDbAccess : IDeleteWorkPackageStepDbAccess
    {
        private readonly EfCoreContext _context;

        public DeleteWorkPackageStepDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Delete(WorkPackageStep workPackageStep)
        {
            _context.WorkPackageStep.Remove(workPackageStep);
        }
    }
}
