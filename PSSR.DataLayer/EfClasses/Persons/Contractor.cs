
using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSSR.DataLayer.EfClasses.Person
{
    public class Contractor:IAuditTracker
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Address { get; private set; }
        public DateTime? ContractDate { get;private set; }
        public DateTime CreatedDate { get; internal set; }
        public DateTime UpdatedDate { get; internal set; }

        //-----------------------------------------
        //Relationships
        public ICollection<Project> Projects { get; private set; }
        private Contractor()
        {
            this.Projects = new List<Project>();
        }

        public Contractor(string name,string phoneNumber,string address,DateTime? contractDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            this.Name = name;
            this.PhoneNumber = phoneNumber;
            this.Address = address;
            this.ContractDate = contractDate;

            this.Projects = new List<Project>();
        }

        public static IStatusGeneric<Contractor> CreateContractor(string name, string phoneNumber, string address, DateTime? contractDate)
        {
            var status = new StatusGenericHandler<Contractor>();
            var newcontractor = new Contractor
            {
                Name = name,
                ContractDate=contractDate,
                Address=address,
                PhoneNumber=phoneNumber
            };

            status.Result = newcontractor;
            return status;
        }

        public IStatusGeneric UpdateContractor(string name, string phoneNumber, string address, DateTime? contractDate)
        {
            var status = new StatusGenericHandler();
            if (string.IsNullOrWhiteSpace(name))
            {
                status.AddError("I'm sorry, but name is empty.");
                return status;
            }

            //All Ok
            this.Name = name;
            this.PhoneNumber = phoneNumber;
            this.Address = address;
            this.ContractDate = contractDate;

            return status;
        }
    }
}
