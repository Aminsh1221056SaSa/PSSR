using PSSR.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.ProjectServices
{
    public class ProjectWBSListDto
    {
        public ProjectWBSListDto()
        {
            this.Childeren = new List<ProjectWBSListDto>();
        }
        public long Id { get;  set; }
        public WBSType Type { get;  set; }
        public long TargetId { get;  set; }
        public float WF { get;  set; }
        public String Name { get; set; }
        public string WBSCode { get;  set; }
        public long? ParentId { get;  set; }
        public float Progress { get; set; }
        public int ActivityCount { get; set; }
        public WfCalculationType CalculationType { get; set; }
        public List<ProjectWBSListDto> Childeren { get; private set; }
    }
}
