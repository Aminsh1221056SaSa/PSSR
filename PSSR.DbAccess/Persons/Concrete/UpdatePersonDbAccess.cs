
using PSSR.DataLayer.EfClasses.Person;
using PSSR.DataLayer.EfCode;
using System.Linq;

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

        public bool HaveAnyPorjects(int personId)
        {
            return _context.Persons.Any(c => c.Id == personId && c.ProjectLink.Any());
        }
    }
}
