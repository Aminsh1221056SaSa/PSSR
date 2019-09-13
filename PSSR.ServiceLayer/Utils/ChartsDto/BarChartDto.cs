using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.Utils.ChartsDto
{
    public class BarChartDto
    {
        public BarChartDto()
        {
            this.Desciplines = new List<string>();
            this.Values = new List<BarChartDetails<string, object>>();
        }
        public List<string> Desciplines { get; set; }
        public List<BarChartDetails<string, object>> Values { get; set; }
    }

    public class BarChartDetails<T,TM>:Dictionary<T,TM>
    {
    }
}
