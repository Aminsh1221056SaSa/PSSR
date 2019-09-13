using BskaGenericCoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.DataLayer.EfClasses.Person
{
    public class Person
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string NationalId { get; private set; }
        public string MobileNumber { get; private set; }

        //-----------------------------------------
        //Relationships
        
        public ICollection<PersonProject> ProjectLink { get; private set; }
        private Person()
        {
            this.ProjectLink = new List<PersonProject>();
        }

        public Person(string firstName,string lastName
            ,string nationalId,string mobileNumber)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentNullException(nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentNullException(nameof(lastName));

            if (string.IsNullOrWhiteSpace(nationalId))
                throw new ArgumentNullException(nameof(nationalId));

            this.FirstName = firstName;
            this.LastName = lastName;
            this.NationalId = nationalId;
            this.MobileNumber = mobileNumber;

            this.ProjectLink = new List<PersonProject>();
        }

        public static IStatusGeneric<Person> CreatePerson(string firstName, string lastName
            , string nationalId, string mobileNumber,Guid[] projectIds)
        {
            var status = new StatusGenericHandler<Person>();
            var person = new Person
            {
                FirstName=firstName,
                LastName=lastName,
                NationalId=nationalId,
                MobileNumber=mobileNumber
            };

            foreach (var ids in projectIds)
            {
                person.ProjectLink.Add(PersonProject.CreatepersonContractor(0, ids).Result);
            }

            status.Result = person;
            return status;
        }

        public IStatusGeneric UpdatePerson(string firstName, string lastName
            , string nationalId, string mobileNumber)
        {
            var status = new StatusGenericHandler();
            if (string.IsNullOrWhiteSpace(firstName))
            {
                status.AddError("I'm sorry, but firstName is empty.");
                return status;
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                status.AddError("I'm sorry, but lastName is empty.");
                return status;
            }

            if (string.IsNullOrWhiteSpace(nationalId))
            {
                status.AddError("I'm sorry, but nationalId is empty.");
                return status;
            }

            //All Ok
            this.FirstName = firstName;
            this.LastName = lastName;
            this.NationalId = nationalId;
            this.MobileNumber = mobileNumber;

            return status;
        }

        public IStatusGeneric AddProjectToperson(Guid projectId)
        {
            var status = new StatusGenericHandler();
            this.ProjectLink.Add(PersonProject.CreatepersonContractor(0, projectId).Result);
            return status;
        }

        public IStatusGeneric RemoveProjectFromperson(PersonProject project)
        {
            var status = new StatusGenericHandler();
            this.ProjectLink.Remove(project);
            return status;
        }
    }
}
