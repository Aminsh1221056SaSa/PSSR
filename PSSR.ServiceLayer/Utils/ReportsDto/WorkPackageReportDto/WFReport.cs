using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.Utils.WorkPackageReportDto
{
    public class WFReportList
    {
        public WFReportList()
        {
            this.Items = new List<WFReport>();
        }
        public long LocationId { get; set; }
        public float LocationWf { get; set; }
        public List<WFReport> Items { get; set; }
    }

    public class WFReport
    {
        public string Name { get; set; }
        public long Id { get; set; }
        public float Wf { get; set; }
        public float Progress { get; set; }
        public float PCT { get; set; }
        public bool WfCalculate { get; set; }
    }
}
 