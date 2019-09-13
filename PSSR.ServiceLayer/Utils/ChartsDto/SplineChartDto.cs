using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.Utils.ChartsDto
{
    public class SplineChartListDto
    {
        public SplineChartListDto()
        {
            this.Values = new List<SplineChartDto>();
        }
        public List<SplineChartDto> Values { get; set; }
    }

    public class SplineChartDto
    {
        public SplineChartDto()
        {
            this.Data = new List<SplineDataDto>();
        }
        public string Name { get; set; }
        public List<SplineDataDto> Data { get; set; }
    }

    public class SplineDataDto
    {
        public string Date { get; set; }
        public int Value { get; set; }
    }
}
