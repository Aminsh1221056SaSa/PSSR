using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Logic.Activityes
{
    public class ActivityPlaneDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ProjectId { get; set; }
        public int DesciplineId { get; set; }
        public int WorkPackageId { get; set; }
        public long SubSystemId { get; set; }
        public int LocationId { get; set; }
    }
}
