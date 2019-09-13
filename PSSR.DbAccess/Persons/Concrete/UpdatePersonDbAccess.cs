
using PSSR.DataLayer.EfClasses.Person;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.Persons.Concrete
{
    public class UpdatePersonDbAccess : IUpdatePersonDbAccess
    {
        private readonly EfCoreContext _context;
        public UpdatePersonDbAccess(EfCoreContext context)
        {
            this._context = context;
        }

        public Person GetPerson(int personId)
        {
            return _context.Find<Person>(personId);
        }
    }
}
