using System;
using System.Collections.Generic;
using System.Text;
using BskaGenericCoreLib;

namespace PSSR.DataLayer.EfClasses.Projects.Activities
{
    public class Punch: IEquatable<Punch>
    {
        public long Id { get; private set; }
        public string Code { get; private set; }
        public string DefectDescription { get; private set; }
        public string OrginatedBy { get; private set; }
        public string ClearBy { get; private set; }
        public string CheckBy { get; private set; }
        public string ApproveBy { get; private set; }
        public string ClearPlan { get; private set; }
        public DateTime OrginatedDate { get; private set; }
        public DateTime? CheckDate { get; private set; }
        public DateTime? ClearDate { get; private set; }
        public bool VendorRequired { get; private set; }
        public bool MaterialRequired { get; private set; }
        public bool EnginerigRequired { get; private set; }
        public string VendorName { get; private set; }
        public string CorectiveAction { get; private set; }
        public int? EstimateMh { get; private set; }
        public int? ActualMh { get; private set; }

        //-----------------------------------------
        //Relationships
        public int PunchTypeId { get; private set; }
        public long ActivityId { get; private set; }
        public int CategoryId { get; private set; }
        public PunchType PunchType { get; private set; }
        public PunchCategory Category { get; private set; }
        public Activity Activity { get; private set; }
        public ICollection<ActivityDocument> ActivityDocuments { get; private set; }
        private Punch()
        {
            this.ActivityDocuments = new List<ActivityDocument>();
        }
        public Punch(string code,string defDesc,string orgBy,string createBy,string checkBy,string approveBy
            ,DateTime orginatedDate,DateTime? checkDate,DateTime? clearDate,int? esMh,int? acMh,bool vendorRequired,
            string vendorName,bool materialRequired,bool enginerRequired,string clearPlan,string corectiveAction,int punchTypeId
            ,long activityId)
        {
            this.Code = code;
            this.DefectDescription = defDesc;
            this.OrginatedBy = orgBy;
            this.ClearBy = createBy;
            this.CheckBy = checkBy;
            this.ApproveBy = approveBy;
            this.OrginatedDate = orginatedDate;
            this.CheckDate = checkDate;
            this.ClearDate = clearDate;
            this.EstimateMh = esMh;
            this.ActualMh = acMh;
            this.VendorRequired = vendorRequired;
            this.VendorName = vendorName;
            this.MaterialRequired = materialRequired;
            this.EnginerigRequired = enginerRequired;
            this.ClearPlan = clearPlan;
            this.CorectiveAction = corectiveAction;
            this.PunchTypeId = punchTypeId;
            this.ActivityId = activityId;

            this.ActivityDocuments = new List<ActivityDocument>();
        }

        public static IStatusGeneric<Punch> CreatePunch(string code, string defDesc, string orgBy, string createBy, string checkBy, string approveBy
            , DateTime orginatedDate, DateTime? checkDate, DateTime? clearDate, int? esMh, int? acMh, bool vendorRequired,
            string vendorName, bool materialRequired, bool enginerRequired, string clearPlan,
            string corectiveAction, int punchTypeId
            , long activityId,int categoryId)
        {
            var status = new StatusGenericHandler<Punch>();

            var punch = new Punch
            {
                Code = code,
                DefectDescription = defDesc,
                OrginatedBy = orgBy,
                ClearBy = createBy,
                CheckBy = checkBy,
                ApproveBy = approveBy,
                OrginatedDate = orginatedDate,
                CheckDate = checkDate,
                ClearDate = clearDate,
                EstimateMh = esMh,
                ActualMh = acMh,
                VendorRequired = vendorRequired,
                VendorName = vendorName,
                MaterialRequired = materialRequired,
                EnginerigRequired = enginerRequired,
                ClearPlan = clearPlan,
                CorectiveAction = corectiveAction,
                PunchTypeId = punchTypeId,
                ActivityId = activityId,
                CategoryId=categoryId
            };

            status.Result = punch;
            return status;
        }

        public IStatusGeneric UpdatePunch(string defDesc, string orgBy, string createBy, string checkBy, string approveBy
            , DateTime orginatedDate, DateTime? checkDate, DateTime? clearDate, int? esMh, int? acMh, bool vendorRequired,
            string vendorName, bool materialRequired, bool enginerRequired, string clearPlan, string corectiveAction)
        {
            var status = new StatusGenericHandler();

            this.DefectDescription = defDesc;
            this.OrginatedBy = orgBy;
            this.ClearBy = createBy;
            this.CheckBy = checkBy;
            this.ApproveBy = approveBy;
            this.OrginatedDate = orginatedDate;
            this.CheckDate = checkDate;
            this.ClearDate = clearDate;
            this.EstimateMh = esMh;
            this.VendorRequired = vendorRequired;
            this.VendorName = vendorName;
            this.MaterialRequired = materialRequired;
            this.EnginerigRequired = enginerRequired;
            this.ClearPlan = clearPlan;
            this.CorectiveAction = corectiveAction;
            this.ActualMh = acMh;

            return status;
        }

        public IStatusGeneric UpdateClear(string clearBy,DateTime? clearDate)
        {
            var status = new StatusGenericHandler();
            if(string.IsNullOrWhiteSpace(clearBy))
            {
                status.AddError("clear by value is null!!", "punch");
            }

            if (!clearDate.HasValue)
            {
                status.AddError("check Date is invalied", "punch");
            }

            if(!status.HasErrors)
            {
                this.ClearBy = clearBy;
                this.ClearDate = clearDate;
            }

            return status;
        }

        public IStatusGeneric UpdateApprove(string checkBy,string approveBy, DateTime? checkDate)
        {
            var status = new StatusGenericHandler();
            if (string.IsNullOrWhiteSpace(checkBy))
            {
                status.AddError("check by value is null!!", "punch");
            }
            if (string.IsNullOrWhiteSpace(approveBy))
            {
                status.AddError("approve by value is null!!", "punch");
            }
            if (!checkDate.HasValue)
            {
                status.AddError("check Date is invalied", "punch");
            }

            if (!status.HasErrors)
            {
                this.CheckBy = checkBy;
                this.ApproveBy = approveBy;
                this.CheckDate = checkDate;
            }

            return status;
        }

        //
        public bool Equals(Punch other)
        {
            if (other == null)
                return base.Equals(other);
            return this.Id == other.Id && this.Code == other.Code;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return base.Equals(obj);
            return Equals(obj as Punch);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.Code, this.Id);
        }
    }
}
