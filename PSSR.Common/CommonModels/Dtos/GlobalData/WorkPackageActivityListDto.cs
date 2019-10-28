using PSSR.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Common.WorkPackageServices
{
    public class WorkPackageActivityListDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int ActivityCount { get; set; }
        public Dictionary<ActivityStatus, int> CountByStatus { get; set; }
    }
}
