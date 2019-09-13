using BskaGenericCoreLib;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSSR.DataLayer.EfClasses.Projects
{
    public class ProjectSubSystem : IAuditTracker
    {
        public long Id { get; private set; }
        public string Code { get; private set; }
        public int PriorityNo { get; private set; }
        public int? SubPriorityNo { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedDate { get; internal set; }
        public DateTime UpdatedDate { get; internal set; }
        //-----------------------------------------
        //Relationships
        public int ProjectSystemId { get; private set; }
        public ProjectSystem ProjectSystem { get; private set; }
        public ICollection<Activity> Activityes { get; private set; }
        private ProjectSubSystem()
        {
            Activityes = new List<Activity>();
        }

        public ProjectSubSystem(string code, string description,int projectSystemId,
            int priorityNo,int? subPriorityNo)
        {
            this.Code = code;
            this.ProjectSystemId = projectSystemId;
            this.Description = description;
            this.PriorityNo = priorityNo;
            this.SubPriorityNo = subPriorityNo;
            //this.PriorityId = priorityId;

            Activityes = new List<Activity>();
        }

        public static IStatusGeneric<ProjectSubSystem> CreateProjectSubSystem(string code, string description, int projectSystemId,
            int priorityNo, int? subPriorityNo)
        {
            var status = new StatusGenericHandler<ProjectSubSystem>();
            var newItem = new ProjectSubSystem
            {
                Code=code,
                Description=description,
                PriorityNo=priorityNo,
                SubPriorityNo=subPriorityNo,
                ProjectSystemId=projectSystemId,
            };
            status.Result = newItem;
            return status;
        }

        public IStatusGeneric UpdateProjectSubSystem(string code, string description,
            int priorityNo, int? subPriorityNo)
        {
            var status = new StatusGenericHandler();

            if (string.IsNullOrWhiteSpace(code))
            {
                status.AddError("I'm sorry, but code is empty.");
                return status;
            }

            //All Ok
            this.Code = code;
            this.Description = description;
            this.PriorityNo = priorityNo;
            this.SubPriorityNo = subPriorityNo;

            return status;
        }
    }
}
