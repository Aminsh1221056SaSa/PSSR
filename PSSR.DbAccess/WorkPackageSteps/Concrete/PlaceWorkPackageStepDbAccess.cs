
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.WorkPackageSteps.Concrete
{
    public class PlaceWorkPackageStepDbAccess : IPlaceWorkPackageStepDbAccess
    {
        private readonly EfCoreContext _context;

        public PlaceWorkPackageStepDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Add(WorkPackageStep workPackageStep)
        {
            _context.WorkPackageStep.Add(workPackageStep);
        }
    }
}
