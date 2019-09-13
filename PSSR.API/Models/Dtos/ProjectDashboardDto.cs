using PSSR.ServiceLayer.ProjectServices;
using PSSR.ServiceLayer.RoadMapServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.API.Models.Dtos
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
        public IEnumerable<ProjectWBSListDto> Locations { get; set; }
    }
}
