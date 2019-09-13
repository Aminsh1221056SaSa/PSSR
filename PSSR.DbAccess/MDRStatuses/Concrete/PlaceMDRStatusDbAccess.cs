
using PSSR.DataLayer.EfClasses.Projects.MDRS;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.MDRStatuses.Concrete
{
    public class PlaceMDRStatusDbAccess : IPlaceMDRStatusDbAccess
    {
        private readonly EfCoreContext _context;
        public PlaceMDRStatusDbAccess(EfCoreContext context)
        {
            this._context = context;
        }
        public void Add(MDRStatus mdrStatus)
        {
            _context.MDRStatus.Add(mdrStatus);
        }
    }
}
