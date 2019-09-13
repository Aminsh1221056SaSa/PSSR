
using PSSR.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Logic.Activityes
{
    public class ActivityStatusUpdateDto
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public ActivityHolBy HoldBy { get; set; }
        public ActivityCondition Condition { get; set; }
        public ActivityStatus Status { get; set; }
    }
}
