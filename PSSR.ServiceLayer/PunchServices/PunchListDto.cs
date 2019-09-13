using PSSR.Common;
using PSSR.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.PunchServices
{
    public class PunchEditableListDto
    {
        public long Id { get; set; }
        public long ActivityId { get; set; }
        public int PunchTypeId { get; set; }
        public string Code { get; set; }
        public string OrginatedBy { get; set; }
        public string Type { get; set; }
        public string OrginatedDate { get; set; }
        public bool IsEditable { get; set; }
        public bool IsClear { get; set; }
        public bool IsApprove { get; set; }
        public string CheckDate { get; set; }
        public string ClearDate { get; set; }
        public string CreatedBy { get; set; }
        public string CheckBy { get; set; }
        public string ApproveBy { get; set; }
    }

    public class PunchListDto
    {
        public long Id { get; set; }
        public long ActivityId { get; set; }
        public int PunchTypeId { get; set; }
        public string Code { get; set; }
        public string OrginatedBy { get; set; }
        public string Type { get; set; }
        public string OrginatedDate { get; set; }
        public string DefectDescription { get; set; }
        public string CreatedBy { get; set; }
        public string CheckBy { get; set; }
        public string ApproveBy { get; set; }
        public int? EstimateMh { get; set; }
        public int? ActualMh { get; set; }
        public string ClearPlan { get; set; }
        public string CheckDate { get; set; }
        public string ClearDate { get; set; }
        public bool VendorRequired { get; set; }
        public bool MaterialRequired { get; set; }
        public bool EnginerigRequired { get; set; }
        public string VendorName { get; set; }
        public string CorectiveAction { get; set; }
        public string ActivityCode { get; set; }
        public float Progress { get; set; }
        public ActivityCondition Condition { get; set; }
        public bool IsEditable { get; set; }
    }
}
