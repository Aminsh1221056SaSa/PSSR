using BskaGenericCoreLib;
using PSSR.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DataLayer.EfClasses.Projects.Activities
{
    public class ActivityStatusHistory
    {
        public long Id { get; private set; }
        public ActivityHolBy HoldBy { get; private set; }
        public string Description { get; private set; }
        public DateTime CreateDate { get; private set; }
        //-----------------------------------------
        //Relationships
        public long ActivityId { get; private set; }
        public Activity Activity { get; private set; }

        public ActivityStatusHistory() { }

        public ActivityStatusHistory(ActivityHolBy holdBy,string description,DateTime createDate,long activityId)
        {
            this.HoldBy = holdBy;
            this.Description = description;
            this.CreateDate = createDate;
            this.ActivityId = activityId;
        }

        public static IStatusGeneric<ActivityStatusHistory> CreateActivityStatusHistory(ActivityHolBy holdBy,
            string description, DateTime createDate)
        {
            var pstatus = new StatusGenericHandler<ActivityStatusHistory>();
            var activityHis = new ActivityStatusHistory
            {
                CreateDate=createDate,
                Description=description,
                HoldBy=holdBy
            };
            pstatus.Result = activityHis;
            return pstatus;
        }
    }
}
