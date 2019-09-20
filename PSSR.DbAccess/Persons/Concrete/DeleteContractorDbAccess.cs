
using PSSR.DataLayer.EfClasses.Person;
using PSSR.DataLayer.EfCode;

namespace PSSR.DbAccess.Persons.Concrete
{
    public class DeletePersonDbAccess : IDeletePersonDbAccess
    {
        private readonly EfCoreContext _context;

        public DeletePersonDbAccess(EfCoreContext context)
        {
            _context = context;
        }

        public void Delete(Person person)
        {
            _context.Persons.Remove(person);
        }
    }
}
