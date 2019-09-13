using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Logic.Punches
{
    public class PunchDto
    {
        public long Id { get;  set; }
        public string Code { get;  set; }
        public string DefectDescription { get;  set; }
        public string OrginatedBy { get;  set; }
        public string CreatedBy { get;  set; }
        public string CheckBy { get;  set; }
        public string ApproveBy { get;  set; }
        public int? EstimateMh { get;  set; }
        public int? ActualMh { get;  set; }
        public string ClearPlan { get;  set; }
        public DateTime OrginatedDate { get;  set; }
        public DateTime? CheckDate { get;  set; }
        public DateTime? ClearDate { get;  set; }
        public bool VendorRequired { get;  set; }
        public bool MaterialRequired { get;  set; }
        public bool EnginerigRequired { get;  set; }
        public string VendorName { get;  set; }
        public string CorectiveAction { get;  set; }
        public int PunchTypeId { get;  set; }
        public int CategoryId { get; set; }
        public long ActivityId { get;  set; }
    }
}
