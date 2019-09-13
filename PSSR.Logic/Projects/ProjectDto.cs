using PSSR.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Logic.Projects
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ContractorId { get; set; }
        public ProjectType Type { get; set; }
    }
}
