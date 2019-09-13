using BskaGenericCoreLib;
using PSSR.DbAccess.Persons;
using PSSR.DbAccess.Projects;
using System;
using System.Linq;

namespace PSSR.Logic.Persons.Concrete
{
    public class UpdatePersonAction : BskaActionStatus,IUpdatePersonAction
    {
        private readonly IUpdatePersonDbAccess _dbAccess;
        private readonly IUpdateProjectDbAccess _projectAccess;

        public UpdatePersonAction(IUpdatePersonDbAccess dbAccess, IUpdateProjectDbAccess projectDbAccess)
        {
            _dbAccess = dbAccess;
            _projectAccess = projectDbAccess;
        }

        public void BizAction(PersonDto inputData)
        {
            var person = _dbAccess.GetPerson(inputData.Id);
            if (person == null)
                AddError("Could not find the person. Someone entering illegal ids?");

            if (string.IsNullOrWhiteSpace(inputData.FirstName))
            {
                AddError("First Name is Required.");
            }

            if (string.IsNullOrWhiteSpace(inputData.LastName))
            {
                AddError("Last Name is Required.");
            }

            if (string.IsNullOrWhiteSpace(inputData.NationalId))
            {
                AddError("National Id is Required.");
            }

            if (string.IsNullOrWhiteSpace(inputData.MobileNumber))
            {
                AddError("Mobile Number is Required.");
            }

            if (!inputData.ProjectIds.Any())
            {
                AddError("Please select some project.");
            }

            IStatusGeneric status = person.UpdatePerson(inputData.FirstName, inputData.LastName, inputData.NationalId, inputData.MobileNumber); ;
            if (!HasErrors)
            {
                var projects = _projectAccess.GetProjectForPerson(person.Id);

                foreach(var p in inputData.ProjectIds)
                {
                    if(projects.Any())
                    {
                        var cp = projects.SingleOrDefault(s => s.Id == p);
                        if (cp == null)
                        {
                            person.AddProjectToperson(p);
                        }
                    }
                    else
                    {
                        person.AddProjectToperson(p);
                    }
                }

                foreach(var p in projects)
                {
                    var cp = inputData.ProjectIds.SingleOrDefault(s => s == p.Id);
                    if (cp == default(Guid))
                    {
                        var personProejct = p.AgentsLink.First(s => s.PersonId == person.Id && s.ProjectId == p.Id);
                        person.RemoveProjectFromperson(personProejct);
                    }
                }
            }
            CombineErrors(status);

            Message = $"person is update: {person.ToString()}.";
        }
    }
}
