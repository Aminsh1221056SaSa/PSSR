using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Common.RoadMapServices
{
    public class WorkPackageListDto
    {
        public int Id { get; set; }
        public string Title { get;  set; }
        public int WF { get; set; }
        public Guid ProjectId { get; set; }
    }
}
