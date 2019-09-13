using BskaGenericCoreLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DataLayer.EfClasses.Projects.Activities
{
    public class ActivityDocument : IAuditTracker
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedDate { get; internal set; }
        public DateTime UpdatedDate { get; internal set; }
        public string FilePath { get;private set; }

        //-----------------------------------------
        //Relationships
        public long ActivityId { get; private set; }
        public long? PunchId { get; private set; }
        public Activity Activity { get; private set; }
        public Punch Punch { get; private set; }

        private ActivityDocument()
        {
        }

        public ActivityDocument(string description,string filePath,long acId,long? punchId)
        {
            this.Description = description;
            this.FilePath = filePath;
            this.ActivityId = acId;
            this.PunchId = punchId;
        }

        public static IStatusGeneric<ActivityDocument> CreateActivityDocument(string description, string filePath, long acId, long? punchId)
        {
            var pstatus = new StatusGenericHandler<ActivityDocument>();
            var acDoc = new ActivityDocument
            {
                ActivityId=acId,
                PunchId=punchId,
                Description=description,
                FilePath=filePath
            };

            pstatus.Result = acDoc;
            return pstatus;
        }
    }
}
