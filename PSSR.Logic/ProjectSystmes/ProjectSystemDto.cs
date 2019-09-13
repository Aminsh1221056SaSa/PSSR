using PSSR.Common;
using System;

namespace PSSR.Logic.ProjectSystmes
{
    public class ProjectSystemDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public SystemType Type { get; set; }
        public Guid ProjectId { get; set; }
    }
}
