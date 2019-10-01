using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System;
using System.Collections.Generic;

namespace PSSR.DataLayer.EfClasses.Management
{
    public class Descipline : IAuditTracker
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedDate { get; internal set; }
        public DateTime UpdatedDate { get; internal set; }
        //public ICollection<ValueUnitDescipline> ValueUnitsLink { get; private set; }
        public ICollection<FormDictionaryDescipline> FormDictionaryLink { get; private set; }
        public ICollection<Activity> Activitys { get; private set; }
        private Descipline()
        {
            //ValueUnitsLink = new List<ValueUnitDescipline>();
            FormDictionaryLink = new List<FormDictionaryDescipline>();
        }

        public Descipline(string name,string description,Guid projectId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            this.Name = name;
            this.Description = description;

            //ValueUnitsLink = new List<ValueUnitDescipline>();
            FormDictionaryLink = new List<FormDictionaryDescipline>();
        }

        public static IStatusGeneric<Descipline> CreateDesciplineFactory(string name,string description,Guid[] projectIds)
        {
            var status = new StatusGenericHandler<Descipline>();
            var descipline = new Descipline
            {
                Description = description,
                Name=name
            };
            
            status.Result = descipline;
            return status;
        }

        public IStatusGeneric UpdateDescipline(string title, string description)
        {
            var status = new StatusGenericHandler();
            if (string.IsNullOrWhiteSpace(title))
            {
                status.AddError("I'm sorry, but Name is empty.");
                return status;
            }

            //All Ok
            this.Name = title;
            this.Description = description;
            return status;
        }
        
        public override string ToString()
        {
            return $"{Name}--{CreatedDate}";
        }
    }
}
