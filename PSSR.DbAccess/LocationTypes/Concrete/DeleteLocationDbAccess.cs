
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.LocationTypes.Concrete
{
    public class DeleteLocationDbAccess : IDeleteLocationDbAccess
    {
        private readonly EfCoreContext _context;

        public DeleteLocationDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Delete(LocationType location)
        {
            _context.LocationTypes.Remove(location);
        }
    }
}
