using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DataLayer.EfClasses.Projects.MDRS;
using System.Collections.Generic;

namespace PSSR.DataLayer.EfClasses.Management
{
    public class WorkPackage
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public ICollection<Activity> Activityes { get; private set; }
        public ICollection<MDRDocument> MDRDocuments { get; private set; }
        public ICollection<FormDictionary> FormDictionarys { get; private set; }
        public ICollection<WorkPackagePunchType> PunchTypes { get; private set; }
        public ICollection<WorkPackageStep> Steps { get; private set; }
        private WorkPackage()
        {
            Activityes = new List<Activity>();
            this.MDRDocuments = new List<MDRDocument>();
            this.FormDictionarys = new List<FormDictionary>();
            this.PunchTypes = new List<WorkPackagePunchType>();
            this.Steps = new List<WorkPackageStep>();
        }

        public WorkPackage(string name)
        {
            this.Name = name;

            Activityes = new List<Activity>();
            this.MDRDocuments = new List<MDRDocument>();
            this.FormDictionarys = new List<FormDictionary>();
            this.PunchTypes = new List<WorkPackagePunchType>();
            this.Steps = new List<WorkPackageStep>();
        }

        public static IStatusGeneric<WorkPackage> CreateRoadMap(string name)
        {
            var status = new StatusGenericHandler<WorkPackage>();
            var newRoadMap = new WorkPackage
            {
                Name = name,
            };
            
            status.Result = newRoadMap;
            return status;
        }

        public IStatusGeneric UpdateRoadMap(string name)
        {
            var status = new StatusGenericHandler();
            if (string.IsNullOrWhiteSpace(name))
            {
                status.AddError("I'm sorry, but name is empty.");
                return status;
            }
            

            //All Ok
            this.Name = name;
            return status;
        }
    }
}
