using PSSR.ServiceLayer.Utils.ChartsDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.Utils.WorkPackageReportDto
{
    public class ManagerDashboardWorkPackageReport
    {
        public IEnumerable<ManagerWorkPackageDto> ActualReport { get; set; }
        public IEnumerable<ManagerWorkPackageDto> PlaneReport { get; set; }
        public IEnumerable<ManagerDashboardGroupDto> GroupReport { get; set; }
        public BarChartDto ChartModels { get; set; }
    }
}
