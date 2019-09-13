using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.Utils.ReportsDto
{
    public class PunchCategoryReportDto
    {
        public PunchCategoryReportDto()
        {
            this.Categories = new List<PunchCategoryDetailsReportDto>();
        }
        public List<PunchCategoryDetailsReportDto> Categories { get; set; }
    }

    public class PunchCategoryDetailsReportDto
    {
        public PunchCategoryDetailsReportDto()
        {
            this.Totals = new List<Tuple<int, int>>();
            this.PunchDetails = new List<KeyValuePair<string, List<Tuple<string,int, int>>>>();
        }
        public string Name { get; set; }
        public int Id { get; set; }

        public List<Tuple<int,int>> Totals { get; set; }
        public List<KeyValuePair<string,List<Tuple<string,int,int>>>> PunchDetails { get; set; }
    }

}
