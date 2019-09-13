using PSSR.ServiceLayer.ProjectServices;
using PSSR.ServiceLayer.RoadMapServices;
using PSSR.ServiceLayer.WorkPackageServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Models
{
    public class ProjectDashboardViewModel
    {
        public ProjectListDto Project { get; set; }
        public IEnumerable<WorkPackageListDto> WorkPackages { get; set; }
    }

    public class ManagerDashboardViewModel
    {
        public IEnumerable<WorkPackageListDto> WorkPackages { get; set; }
        public ProjectListDto Project { get; set; }
        public IEnumerable<ProjectWBSListDto> Locations { get; set; }
    }
}
