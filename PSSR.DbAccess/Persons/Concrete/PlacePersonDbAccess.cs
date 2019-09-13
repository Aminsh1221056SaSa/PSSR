
using PSSR.DataLayer.EfClasses.Person;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.Persons.Concrete
{
    public class PlacePersonDbAccess : IPlacePersonDbAccess
    {
        private readonly EfCoreContext _context;
        public PlacePersonDbAccess(EfCoreContext context)
        {
            this._context = context;
        }

        public void Add(Person person)
        {
            _context.Persons.Add(person);
        }
    }
}
