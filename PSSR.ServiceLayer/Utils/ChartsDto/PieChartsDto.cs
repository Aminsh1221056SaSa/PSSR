using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.Utils.ChartsDto
{
    public class PieChartsListDto
    {
        public PieChartsListDto()
        {
            this.Values = new List<PieChartDto>();
        }
        public List<PieChartDto> Values { get; set; }
    }

    public class PieChartDto
    {
        public string Name { get; set; }
        public double Y { get; set; }
        public bool Sliced { get; set; }
        public bool Selected { get; set; }
    }
}
