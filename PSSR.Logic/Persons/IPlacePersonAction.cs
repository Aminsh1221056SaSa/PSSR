using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Person;

namespace PSSR.Logic.Persons
{
    public interface IPlacePersonAction : IGenericActionWriteDb<PersonDto, Person> { }
}
