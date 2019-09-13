using BskaGenericCoreLib;
using PSSR.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSSR.DataLayer.EfClasses.Projects
{
    public class ProjectSystem : IAuditTracker
    {
        public int Id { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public SystemType Type { get; private set; }
        public DateTime CreatedDate { get; internal set; }
        public DateTime UpdatedDate { get; internal set; }
        
        //-----------------------------------------
        //Relationships
        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; }

        public ICollection<ProjectSubSystem> ProjectSubSystems { get; private set; }
        private ProjectSystem()
        {
            ProjectSubSystems = new List<ProjectSubSystem>();
        }

        public ProjectSystem(string code, string description,SystemType type, Guid projectId)
        {
            this.Code = code;
            this.ProjectId = projectId;
            this.Type = type;
            this.Description = description;

            ProjectSubSystems = new List<ProjectSubSystem>();
        }

        public static IStatusGeneric<ProjectSystem> CreateProjectSystem(string code, string description, SystemType type, Guid projectId)
        {
            var status = new StatusGenericHandler<ProjectSystem>();

            var newSystem = new ProjectSystem
            {
                Code = code,
                Description = description,
                ProjectId = projectId,
                Type = type
            };

            status.Result = newSystem;
            return status;
        }

        public IStatusGeneric UpdateProjectSystem(string code, string description)
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
            return status;
        }
    }
}
