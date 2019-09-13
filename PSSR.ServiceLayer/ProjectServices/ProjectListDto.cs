using PSSR.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.ProjectServices
{
    public class ProjectListDto
    {
        public Guid Id { get;  set; }
        public string Description { get;  set; }
        public string StartDate { get;  set; }
        public string EndDate { get;  set; }
        public int ContractorId { get; set; }
        public string ContractorName { get; set; }
        public int ElapsedDate { get; set; }
        public int RemainedDate { get; set; }

        public int SystemsCount { get; set; }
        public int SubSystemsCount { get; set; }
        public int ActivitysCount { get; set; }
        public Common.ProjectType Type { get; set; }
    }
}
