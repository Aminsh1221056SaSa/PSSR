using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Logic.Contractors
{
    public class ContractorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime? ContractDate { get; set; }
    }
}
