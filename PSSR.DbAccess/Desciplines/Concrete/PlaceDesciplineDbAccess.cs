
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.Desciplines.Concrete
{
    public class PlaceDesciplineDbAccess : IPlaceDesciplineDbAccess
    {
        private readonly EfCoreContext _context;

        public PlaceDesciplineDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Add(Descipline descipline)
        {
            _context.Desciplines.Add(descipline);
        }
    }
}
