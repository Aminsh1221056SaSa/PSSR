using PSSR.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.ProjectSystemServices
{
    public class ProjectSystemListDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public SystemType Type { get; set; }
        public Guid ProjectId { get; set; }
    }
}
