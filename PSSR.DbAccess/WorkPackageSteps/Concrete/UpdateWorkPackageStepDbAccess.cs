
using System.Linq;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.WorkPackageSteps.Concrete
{
    public class UpdateWorkPackageStepDbAccess : IUpdateWorkPackageStepDbAccess
    {
        private readonly EfCoreContext _context;

        public UpdateWorkPackageStepDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public WorkPackageStep GetWorkPackageStep(int workPackagestepId)
        {
            var item = _context.Find<WorkPackageStep>(workPackagestepId);
            return item;
        }

        public bool HaveAnyActivity(int id)
        {
            return _context.WorkPackageStep.Where(s => s.Id == id).Any(s => s.Activities.Any());
        }
    }
}
