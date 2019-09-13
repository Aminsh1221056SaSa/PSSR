using BskaGenericCoreLib;
using PSSR.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PSSR.DataLayer.EfClasses.Projects
{
    [Serializable]
    public class ProjectWBS : IEquatable<ProjectWBS>
    {
        public long Id { get;private set; }
        public WBSType Type { get; private set; }
        public long TargetId { get; private set; }
        public float WF { get; private set; }
        public string Name { get; private set; }
        public string WBSCode { get; set; }
        public WfCalculationType CalculationType { get; private set; }

        //relationship
        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; }
        public long? ParentId { get; private set; }
        public ProjectWBS Parent { get; private set; }

        public ICollection<ProjectWBS> Childeren { get; private set; }

        //not mapped
        [NotMapped]
        public float Progress { get; set; }
        [NotMapped]
        public int ActivityCount { get; set; }

        private ProjectWBS()
        {
            this.Childeren = new List<ProjectWBS>();
            this.Progress = 0;
            this.ActivityCount = 0;
        }

        public ProjectWBS(WBSType type,long targetId,float wf,string wbsCode,Guid projectId,long? parentId,string name,WfCalculationType caltype)
        {
            this.Id = targetId;
            this.Type = type;
            this.TargetId = targetId;
            this.WF = wf;
            this.WBSCode = wbsCode;
            this.ProjectId = projectId;
            this.ParentId = parentId;
            this.Name = name;
            this.CalculationType = caltype;
            this.Progress = 0;
            this.ActivityCount = 0;
            this.Childeren = new List<ProjectWBS>();
        }

        public static IStatusGeneric<ProjectWBS> CreateProjectWBS(WBSType type, long targetId, float wf, string wbsCode,
            Guid projectId, long? parentId,string name,WfCalculationType calType)
        {
            var status = new StatusGenericHandler<ProjectWBS>();

            var newItem = new ProjectWBS
            {
                ParentId = parentId,
                ProjectId = projectId,
                TargetId = targetId,
                Type = type,
                WBSCode = wbsCode,
                WF = wf,
                Name = name,
                CalculationType=calType,
            };

            status.Result = newItem;
            return status;
        }

        public void SetParent(ProjectWBS parent)
        {
            this.Parent = parent;
        }

        public IStatusGeneric UpdateProject(string wbsCode,WfCalculationType calType)
        {
            var status = new StatusGenericHandler();

            //All Ok
            this.WBSCode = wbsCode;
            this.CalculationType = calType;

            return status;
        }

        public IStatusGeneric UpdateProjectWF(float wf)
        {
            var status = new StatusGenericHandler();

            //All Ok
            this.WF = wf;

            return status;
        }

        public void LocalWfUpdate(float wf)
        {
            this.WF = wf;
        }

        public static ProjectWBS CreateProjectWBSToParent(WBSType type, long targetId, float wf, string wbsCode,
           Guid projectId, string name,ProjectWBS parent, WfCalculationType calType)
        {
            var newItem = new ProjectWBS
            {
                ProjectId = projectId,
                TargetId = targetId,
                Type = type,
                WBSCode = wbsCode,
                WF = wf,
                Name = name,
                Parent=parent,
                ParentId=parent.Id
            };
            newItem.Childeren = new List<ProjectWBS>();
             return newItem;
        }
        //
        public bool Equals(ProjectWBS other)
        {
            if (other == null)
                return base.Equals(other);
            return this.Id == other.Id && this.Name == other.Name;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return base.Equals(obj);
            return Equals(obj as ProjectWBS);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
