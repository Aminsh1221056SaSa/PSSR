
using PSSR.DataLayer.EfClasses.Person;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.Contractors.Concrete
{
    public class PlaceContractorDbAccess : IPlaceContractorDbAccess
    {
        private readonly EfCoreContext _context;

        public PlaceContractorDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Add(Contractor contractor)
        {
            _context.Contractors.Add(contractor);
        }
    }
}
