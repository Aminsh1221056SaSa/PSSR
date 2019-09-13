using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.PersonService
{
    public class PersonListDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public string MobileNumber { get; set; }
        public IEnumerable<string> Projects { get; set; }
        public IEnumerable<Guid> CurrentProjects { get; set; }
    }

    public class PersonSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NationalId { get; set; }
    }
}
