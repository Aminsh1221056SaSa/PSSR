using BskaGenericCoreLib;
using PSSR.Common;
using PSSR.DataLayer.EfClasses.Management;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSSR.DataLayer.EfClasses.Projects.Activities
{
    public class Activity : IAuditTracker,IEquatable<Activity>
    {
        public long Id { get; private set; }
        public string ActivityCode { get; private set; }
        public string TagNumber { get; private set; }
        public string TagDescription { get; private set; }
        public float Progress { get; private set; }
        public float WeightFactor { get; private set; }
        public float ValueUnitNum { get; private set; }
        public int EstimateMh { get; private set; }
        public int ActualMh { get; private set; }
        public ActivityStatus Status { get; private set; } 
        public ActivityCondition Condition { get; private set; }

        public DateTime? ActualStartDate { get; private set; }
        public DateTime? ActualEndDate { get; private set; }
        public DateTime? PlanStartDate { get; private set; }
        public DateTime? PlanEndDate { get; private set; }

        public DateTime CreatedDate { get; internal set; }
        public DateTime UpdatedDate { get; internal set; }

        //value unit
        public int ValueUnitId { get; private set; }
        public ValueUnit ValueUnit { get; private set; }
        //-----------------------------------------
        //Relationships
        //workPackage
        public int? WorkPackageId { get; private set; }
        public WorkPackage WorkPackage { get; private set; }

        //location
        public int? LocationId { get; private set; }
        public LocationType Location { get; private set; }

        //subsystem
        public long? SubsytemId { get; private set; }
        public ProjectSubSystem SubSystem { get; private set; }

        //desciplines
        public int? DesciplineId { get; private set; }
        public Descipline Descipline { get; private set; }

        //formdictionary
        public long? FormDictionaryId { get; private set; }
        public FormDictionary FormDictionary { get; private set; }

        //workpackagestep
        public int? WorkPackageStepId { get; private set; }
        public WorkPackageStep WorkPackageStep { get; private set; }

        public ICollection<Punch> Punchs { get; private set; }
        public ICollection<ActivityStatusHistory> StatusHistory { get; private set; }
        public ICollection<ActivityDocument> ActivityDocuments { get; private set; }

        private Activity()
        {
            this.Punchs = new List<Punch>();
            this.StatusHistory = new List<ActivityStatusHistory>();
            this.ActivityDocuments = new List<ActivityDocument>();
        }

        public Activity(string tagNumber,string tagDes,int progress,int weightFactor
            ,float valueUnitNum,int estimateMh,int actualMh, ActivityStatus status
            ,DateTime? acStartDate,DateTime? acEndDate
            ,DateTime? planStartDate,DateTime? planEndDate,long formDicId,int valueUnitId
            ,int workpackageId,int locationId,long subsystemId, ActivityCondition condition,string acCode
            , int desciplineId,int wStepId)
        {
            this.TagNumber = tagNumber;
            this.TagDescription = tagDes;
            this.Progress = progress;
            this.WeightFactor = weightFactor;
            this.ValueUnitNum = valueUnitNum;
            this.EstimateMh = estimateMh;
            this.ActualMh = actualMh;
            this.Status = status;
            this.ActualStartDate = acStartDate;
            this.ActualEndDate = acEndDate;
            this.PlanStartDate = planStartDate;
            this.PlanEndDate = planEndDate;
            this.FormDictionaryId = formDicId;
            this.ValueUnitId = valueUnitId;
            this.WorkPackageId = workpackageId;
            this.LocationId = locationId;
            this.SubsytemId = subsystemId;
            this.Condition = condition;
            this.ActivityCode = acCode;
            this.DesciplineId = desciplineId;
            this.WorkPackageStepId = wStepId;

            this.Punchs = new List<Punch>();
            this.StatusHistory = new List<ActivityStatusHistory>();
            this.ActivityDocuments = new List<ActivityDocument>();
        }

        public static IStatusGeneric<Activity> CreateActivity(string tagNumber, string tagDes, float progress, float weightFactor
            , float valueUnitNum, int estimateMh, int actualMh, ActivityStatus status
            , DateTime? acStartDate, DateTime? acEndDate
            , DateTime? planStartDate, DateTime? planEndDate, long formDicId, int valueUnitId
            , int workpackageId, int locationId, long subsystemId,
            ActivityCondition condition, string acCode, int desciplineId, int wStepId)
        {
            var pstatus = new StatusGenericHandler<Activity>();
            var newActivity = new Activity
            {
                ActivityCode=acCode,
                TagNumber = tagNumber,
                TagDescription = tagDes,
                Progress = progress,
                WeightFactor = weightFactor,
                ValueUnitNum = valueUnitNum,
                EstimateMh = estimateMh,
                ActualMh = actualMh,
                Status = status,
                ActualStartDate = acStartDate,
                ActualEndDate = acEndDate,
                PlanStartDate = planStartDate,
                PlanEndDate = planEndDate,
                FormDictionaryId = formDicId,
                ValueUnitId = valueUnitId,
                WorkPackageId = workpackageId,
                LocationId = locationId,
                SubsytemId = subsystemId,
                Condition=condition,
                DesciplineId=desciplineId,
                UpdatedDate=DateTime.Now,
                CreatedDate=DateTime.Now,
                WorkPackageStepId=wStepId
            };
            
            //newActivity.StatusHistory.Add(ActivityStatusHistory.CreateActivityStatusHistory(ActivityHolBy.NoHold, null, DateTime.Now).Result);

            pstatus.Result = newActivity;
            return pstatus;
        }

        public IStatusGeneric UpdateActivity(string tagNumber, string tagDes
            , float valueUnitNum, int estimateMh, int actualMh
            , DateTime? acStartDate, DateTime? acEndDate
            , ActivityCondition condition, ActivityStatus status)
        {
            var pstatus = new StatusGenericHandler();

            this.TagNumber = tagNumber;
            this.TagDescription = tagDes;
            this.ValueUnitNum = valueUnitNum;
            this.EstimateMh = estimateMh;
            this.ActualMh = actualMh;
            this.ActualStartDate = acStartDate;
            this.ActualEndDate = acEndDate;
            this.Condition = condition;
            this.Status = status;

            return pstatus;
        }

        public IStatusGeneric UpdateActivityStatus(ActivityStatus status,
            ActivityHolBy acHoldBy,ActivityCondition condition)
        {
            var pstatus = new StatusGenericHandler();

            if (this.Status == Common.ActivityStatus.Done)
            {
                pstatus.AddError("activity status unable to edit!!!");
            }

            if (condition == Common.ActivityCondition.Hold)
            {
                if (acHoldBy== Common.ActivityHolBy.NoHold)
                {
                    pstatus.AddError("Please select a hold attribute!!!");
                }
            }

            if(!pstatus.HasErrors)
            {
                if (status == ActivityStatus.Ongoing)
                {
                    this.ActualStartDate = DateTime.Now;
                }
                else if (status == ActivityStatus.Done)
                {
                    this.Progress = 100;
                    this.ActualEndDate = DateTime.Now;
                }
                else if (status == ActivityStatus.Reject || status==ActivityStatus.Delete)
                {
                    this.Progress = 0;
                    this.WeightFactor = 0;
                }

                this.Condition = condition;
                this.Status = status;

                string description = $"{status} - {condition}";

                this.StatusHistory.Add(ActivityStatusHistory.CreateActivityStatusHistory(acHoldBy, description, DateTime.Now).Result);
            }

            return pstatus;
        }

        public void UpdateActivityWf(float wf)
        {
            this.WeightFactor = wf;
        }

        public void UpdateActivityProgress(float progress)
        {
            this.Progress = progress;
        }

        public void UpdateActivityPlane(DateTime? startDate,DateTime? EndDate)
        {
            this.PlanStartDate = startDate;
            this.PlanEndDate = EndDate;
        }

        public IStatusGeneric<ActivityDocument> CreateActivityDocument(string description, string filePath, long acId, long? punchId)
        {
            var status = ActivityDocument.CreateActivityDocument(description, filePath, acId, punchId);
            this.ActivityDocuments.Add(status.Result);
            return status;
        }

        //
        public bool Equals(Activity other)
        {
            if (other == null)
                return base.Equals(other);
            return this.Id == other.Id && this.ActivityCode == other.ActivityCode;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return base.Equals(obj);
            return Equals(obj as Activity);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.ActivityCode, this.Id);
        }
    }
}
