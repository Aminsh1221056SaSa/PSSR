using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DataLayer.EfClasses.Person
{
    public class PersonProject
    {
        public int PersonId { get; private set; }
        public Guid ProjectId { get; private set; }

        public Project Project { get; private set; }
        public Person Person { get; private set; }

        private PersonProject() { }
        public PersonProject(Project project,Person person)
        {
            this.Project = project;
            this.Person = person;
        }

        public static IStatusGeneric<PersonProject> CreatepersonContractor(int personId, Guid projectId)
        {
            var status = new StatusGenericHandler<PersonProject>();

            var newItem = new PersonProject
            {
               PersonId=personId,
               ProjectId=projectId
            };

            status.Result = newItem;
            return status;
        }
    }
}
