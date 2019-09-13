using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.Utils.WorkPackageReportDto
{
    public class ManagerWorkPackageDto
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public float Value { get; set; }
        public bool IsBolded { get; set; }
        public bool IsTitle { get; set; }
    }

    public class ManagerDashboardGroupDto
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public int Total { get; set; }
        public int Done { get; set; }
    }
}
