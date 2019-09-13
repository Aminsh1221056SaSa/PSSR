
using PSSR.DataLayer.EfClasses.Person;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PSSR.ServiceLayer.PersonService.QueryObjects
{
    public enum PersonFilterBy
    {
        [Display(Name = "All")]
        NoFilter = 0,
        [Display(Name = "By Project")]
        Project
    }

    public static class PersonListDtoFilter
    {
        public static IQueryable<Person> FiltePersonBy(
            this IQueryable<Person> persons,
            PersonFilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return persons;

            switch (filterBy)
            {
                case PersonFilterBy.NoFilter:
                    return persons;

                case PersonFilterBy.Project:
                    Guid pid = Guid.Parse(filterValue);
                    return persons.Where(x =>
                          x.ProjectLink.Any(s=>s.ProjectId==pid));

                default:
                    throw new ArgumentOutOfRangeException
                        (nameof(filterBy), filterBy, null);
            }
        }
    }
}
