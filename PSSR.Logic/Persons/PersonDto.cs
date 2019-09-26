using PSSR.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Logic.Persons
{
    public class PersonDto
    {
        public int Id { get;  set; }
        public string FirstName { get;  set; }
        public string LastName { get;  set; }
        [NationalCode(ErrorMessage ="National id is invalid.")]
        public string NationalId { get;  set; }
        public string MobileNumber { get; set; }

        public Guid[] ProjectIds { get; set; }
    }
}
