using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DataLayer.EfClasses.Management
{
    public class WorkPackageStep
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int WorkPackageId { get; private set; }
        public virtual WorkPackage WorkPackage { get; private set; }
        public virtual ICollection<Activity> Activities { get; private set; }

        public WorkPackageStep() { }
        public WorkPackageStep(string title,string description,int workPackageId)
        {
            this.Title = title;
            this.Description = description;
            this.WorkPackageId = workPackageId;

            this.Activities = new List<Activity>();
        }

        public static IStatusGeneric<WorkPackageStep> CreateWorkPackageStep(string title, string description, int workPackageId)
        {
            var status = new StatusGenericHandler<WorkPackageStep>();

            var newItem = new WorkPackageStep
            {
                WorkPackageId = workPackageId,
                Title = title,
                Description = description,
            };

            status.Result = newItem;
            return status;
        }

        public IStatusGeneric UpdateWorkPackageStep(string title,string description)
        {
            var status = new StatusGenericHandler();
            if (string.IsNullOrWhiteSpace(title))
            {
                status.AddError("I'm sorry, but title is empty.");
                return status;
            }

            //All Ok
            this.Title = title;
            this.Description = description;
            return status;
        }
    }
}
 