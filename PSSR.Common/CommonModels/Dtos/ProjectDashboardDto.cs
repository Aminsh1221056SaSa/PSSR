
using PSSR.Common.ProjectServices;
using PSSR.Common.RoadMapServices;
using System.Collections.Generic;

namespace PSSR.Common.Models.Dtos
{
    public class ProjectDashboardDto
    {
        public ProjectListDto Project { get; set; }
        public IEnumerable<WorkPackageListDto> WorkPackages { get; set; }
    }

    public class ManagerDashboardDto
    {
        public IEnumerable<WorkPackageListDto> WorkPackages { get; set; }
        public ProjectListDto Project { get; set; }
    }
}
