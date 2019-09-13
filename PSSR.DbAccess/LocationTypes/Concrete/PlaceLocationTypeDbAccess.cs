
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.LocationTypes.Concrete
{
    public class PlaceLocationTypeDbAccess : IPlaceLocationTypeDbAccess
    {
        private readonly EfCoreContext _context;
        public PlaceLocationTypeDbAccess(EfCoreContext context)
        {
            this._context = context;
        }

        public void Add(LocationType location)
        {
            _context.LocationTypes.Add(location);
        }
    }
}
