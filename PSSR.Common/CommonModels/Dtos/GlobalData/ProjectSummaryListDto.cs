using PSSR.Common;
using System;

namespace PSSR.Common.ProjectServices
{
    public class ProjectSummaryListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContractorName { get; set; }
        public ProjectType Type { get; set; }
    }
}
