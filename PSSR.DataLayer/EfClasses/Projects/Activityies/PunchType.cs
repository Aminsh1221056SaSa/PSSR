using BskaGenericCoreLib;
using System;
using System.Collections.Generic;

namespace PSSR.DataLayer.EfClasses.Projects.Activities
{
    public class PunchType
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        //-----------------------------------------
        //Relationships
        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; }
        public ICollection<Punch> Punches { get; private set; }
        public ICollection<WorkPackagePunchType> WorkPackages { get; private set; }

        private PunchType()
        {
            this.WorkPackages = new List<WorkPackagePunchType>();
        }
        public PunchType(string name,Guid projectId)
        {
            this.Name = name;
            this.ProjectId = projectId;

            this.WorkPackages = new List<WorkPackagePunchType>();
            this.Punches = new List<Punch>();
        }

        public static IStatusGeneric<PunchType> CreatePunchType(string name,Guid projectId,Dictionary<int,float> workPackagepr)
        {
            var status = new StatusGenericHandler<PunchType>();
            var punchType = new PunchType
            {
                Name = name,
                ProjectId=projectId
            };

            foreach(var dic in workPackagepr)
            {
                punchType.WorkPackages.Add(WorkPackagePunchType.CreateWorkPackagePunchType(dic.Value,0,dic.Key).Result);
            }

            status.Result = punchType;
            return status;
        }

        public IStatusGeneric UpdatePunchType(string name)
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
