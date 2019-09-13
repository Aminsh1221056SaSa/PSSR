using System;

namespace PSSR.Logic.RoadMaps
{
    public class ProjectWorkPackageListDto
    {
        public int Id { get;  set; }
        public string Name { get;  set; }
        public Guid[] ProjectIds { get; set; }
    }
}
