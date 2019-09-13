using BskaGenericCoreLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DataLayer.EfClasses.Projects.Activities
{
    public class PunchCategory
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        //-----------------------------------------
        //Relationships
        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; }
        public ICollection<Punch> Punches { get; private set; }

        private PunchCategory()
        {
            this.Punches = new List<Punch>();
        }
        public PunchCategory(string name,Guid projectId)
        {
            this.Name = name;
            this.ProjectId = projectId;
        
            this.Punches = new List<Punch>();
        }

        public static IStatusGeneric<PunchCategory> CreatePunchCategory(string name, Guid projectId)
        {
            var status = new StatusGenericHandler<PunchCategory>();
            var punchType = new PunchCategory
            {
                Name = name,
                ProjectId = projectId
            };

            status.Result = punchType;
            return status;
        }

        public IStatusGeneric UpdatePunchCategory(string name)
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
