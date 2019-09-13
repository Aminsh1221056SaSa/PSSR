using PSSR.DataLayer.EfClasses;
using PSSR.DataLayer.EfClasses.Person;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DbAccess.Persons
{
    public interface IUpdatePersonDbAccess
    {
        Person GetPerson(int personId);
    }
}
