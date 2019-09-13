using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Person;
using PSSR.DbAccess.Persons;
using System.Linq;

namespace PSSR.Logic.Persons.Concrete
{
    public class PlacePersonAction : BskaActionStatus, IPlacePersonAction
    {
        private readonly IPlacePersonDbAccess _dbAccess;
        public PlacePersonAction(IPlacePersonDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public Person BizAction(PersonDto inputData)
        {
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

            if(!inputData.ProjectIds.Any())
            {
                AddError("Please select some project.");
            }

            var desStatus = Person.CreatePerson(inputData.FirstName,inputData.LastName,inputData.NationalId,inputData.MobileNumber
                ,inputData.ProjectIds);

            CombineErrors(desStatus);

            if (!HasErrors)
                _dbAccess.Add(desStatus.Result);

            return HasErrors ? null : desStatus.Result;
        }
    }
}
