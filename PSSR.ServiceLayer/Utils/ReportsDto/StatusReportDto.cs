using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.Utils.ReportsDto
{
    public class StatusReportDto
    {
        public StatusReportDto()
        {
            this.StatusDetails = new List<StatusReportDetailDto>();
        }
        public string Priority { get; set; }
        public string SubSystemDesc { get; set; }
        public string SubSystemCode { get; set; }
        public string StatusDesc { get; set; }
        public List<StatusReportDetailDto> StatusDetails { get; set; }
    }

    public class StatusReportDetailDto
    {
        public string Descipline { get; set; }
        public int RemainSheet { get; set; }
        public string RemainPunchDesc1 { get; set; }
        public int RemainPunchNum1 { get; set; }
        public string StatusDesc { get; set; }
    }
}
