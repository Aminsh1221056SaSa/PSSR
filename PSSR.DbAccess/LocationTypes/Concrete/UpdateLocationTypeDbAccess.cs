
using PSSR.DataLayer.EfClasses;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;
using System.Linq;

namespace PSSR.DbAccess.LocationTypes.Concrete
{
    public class UpdateLocationTypeDbAccess : IUpdateLocationTypeDbAccess
    {
        private readonly EfCoreContext _context;
        public UpdateLocationTypeDbAccess(EfCoreContext context)
        {
            this._context = context;
        }

        public LocationType GetLocationType(int locationId)
        {
            return _context.Find<LocationType>(locationId);
        }

        public bool HaveAnyActivity(int id)
        {
            return _context.LocationTypes.Where(s => s.Id == id).Any(s => s.Activityes.Any());
        }
    }
}
