using PSSR.ServiceLayer.PunchTypeServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.Utils.ReportsDto
{
    public class ClearPunchReportDto
    {
        public ClearPunchReportDto()
        {
            Items = new List<ClearPunchDetailsDto>();
        }
        public IEnumerable<PunchTypeListDto> PunchType { get; set; }
        public List<ClearPunchDetailsDto> Items { get; set; }
    }

    public class ClearPunchDetailsDto
    {
        public ClearPunchDetailsDto()
        {
            this.Totals = new List<int>();
            this.ClearPunch = new List<int>();
            this.Remain = new List<int>();
        }
        public string DesciplineName { get; set; }
        public int DesciplineId { get; set; }
        public List<int> Totals { get; set; }
        public List<int> ClearPunch { get; set; }
        public List<int> Remain { get; set; }
    }
}
