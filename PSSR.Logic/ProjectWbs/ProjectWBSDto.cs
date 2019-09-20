using PSSR.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Logic.Projects
{
    public class ProjectWBSDto
    {
        public long Id { get;  set; }
        public WBSType Type { get;  set; }
        public long TargetId { get;  set; }
        public float WF { get;  set; }
        public string WBSCode { get;  set; }
        public string Name { get; set; }
        public WfCalculationType CalculationType { get; set; }

        //relationship
        public Guid ProjectId { get;  set; }
        public long? ParentId { get; set; }
    }
}
