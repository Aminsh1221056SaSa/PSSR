using PSSR.Common.ProjectServices;
using PSSR.Common.RoadMapServices;
using System.Collections.Generic;

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
    }
}
