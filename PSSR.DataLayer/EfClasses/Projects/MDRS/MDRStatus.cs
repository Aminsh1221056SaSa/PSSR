using BskaGenericCoreLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DataLayer.EfClasses.Projects.MDRS
{
    public class MDRStatus
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public float Wf { get; private set; }
        public string Description { get; private set; }
        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; }

        public ICollection<MDRStatusHistory> MDRHistoryies { get; private set; }

        private MDRStatus() { }
        public MDRStatus(string name,float wf, string description)
        {
            this.Name = name;
            this.Wf = wf;
            this.Description = description;

            this.MDRHistoryies = new List<MDRStatusHistory>();
        }

        public static IStatusGeneric<MDRStatus> CreateMDRStatus(string name, float wf,Guid projectId,string description)
        {
            var status = new StatusGenericHandler<MDRStatus>();
            var mdrStatus = new MDRStatus
            {
                Wf = wf,
                Name = name,
                ProjectId=projectId,
                Description=description
            };

            status.Result = mdrStatus;
            return status;
        }

        public IStatusGeneric UpdateMDRStatus(string name, float wf, string description)
        {
            var status = new StatusGenericHandler();
            if (string.IsNullOrWhiteSpace(name))
            {
                status.AddError("I'm sorry, but name is empty.");
                return status;
            }

            this.Name = name;
            this.Wf = wf;
            this.Description = description;

            return status;
        }
    }
}
