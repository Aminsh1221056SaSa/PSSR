using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.ProjectServices
{
    public class ProjectWBSTableDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string WbsCode { get; set; }
        public float Wf { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int TaskCount { get; set; }
    }
}
