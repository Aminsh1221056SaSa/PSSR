
using PSSR.DataLayer.EfClasses.Person;

namespace PSSR.DbAccess.Persons
{
    public interface IDeletePersonDbAccess
    {
        void Delete(Person person);
    }
}
