using PSSR.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.ActivityServices
{
    public class PlanActivityDto
    {
        public long Id { get; set; }
        public string ActivityCode { get; set; }
        public ActivityStatus Status { get; set; }
        public float WF { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
