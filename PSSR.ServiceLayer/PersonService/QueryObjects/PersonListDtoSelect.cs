using PSSR.DataLayer.EfClasses;
using PSSR.DataLayer.EfClasses.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.PersonService.QueryObjects
{
    public static class PersonListDtoSelect
    {
        public static IQueryable<PersonListDto>
           MapPersonToDto(this IQueryable<Person> projectSystmes)
        {
            return projectSystmes.Select(p => new PersonListDto
            {
                Id = p.Id,
                FirstName=p.FirstName,
                LastName=p.LastName,
                MobileNumber=p.MobileNumber,
                NationalId=p.NationalId,
                Projects=p.ProjectLink.Select(s=>s.Project.Description)
            });
        }
    }
}
