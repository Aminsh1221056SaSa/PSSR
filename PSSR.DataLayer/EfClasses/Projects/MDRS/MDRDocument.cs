using BskaGenericCoreLib;
using PSSR.Common;
using PSSR.DataLayer.EfClasses.Management;
using System;
using System.Collections.Generic;

namespace PSSR.DataLayer.EfClasses.Projects.MDRS
{
    public class MDRDocument : IAuditTracker
    {
        public long Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedDate { get; internal set; }
        public DateTime UpdatedDate { get; internal set; }
        public string Code { get; private set; }
        public MDRDocumentType Type { get;private set; }
        public bool IsCompleted { get; private set; }
        //-----------------------------------------
        //Relationships
        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; }
        public int WorkPackageId { get; private set; }
        public WorkPackage WorkPackage { get; private set; }
        public ICollection<MDRDocumentComment> MDRDocumentComments { get; private set; }
        public ICollection<MDRStatusHistory> MDRStatusHistoryies { get; private set; }

        private MDRDocument()
        {
            this.MDRDocumentComments = new List<MDRDocumentComment>();
            this.MDRStatusHistoryies = new List<MDRStatusHistory>();
        }

        public MDRDocument(string title,string description,int workPackageId,string code, Guid projectId, MDRDocumentType type)
        {
            this.Title = title;
            this.Description = description;
            this.WorkPackageId = workPackageId;
            this.Code = code;
            this.ProjectId = projectId;
            this.Type = type;

            this.MDRDocumentComments = new List<MDRDocumentComment>();
            this.MDRStatusHistoryies = new List<MDRStatusHistory>();
        }

        public static IStatusGeneric<MDRDocument> CreateMDRDocument(string title,
            string description, int WorkPackageId,string code,int commentStatusId,Guid projectId, MDRDocumentType type)
        {
            var pstatus = new StatusGenericHandler<MDRDocument>();

            var newMDRDoc = new MDRDocument
            {
                Description=description,
                WorkPackageId = WorkPackageId,
                Title=title,
                Code=code,
                ProjectId=projectId,
                Type= type
            };
            
            pstatus.Result = newMDRDoc;
            return pstatus;
        }

        public IStatusGeneric UpdateMDRDocument( string title, string description, 
            int WorkPackageId,string code, MDRDocumentType type)
        {
            var pstatus = new StatusGenericHandler();

            this.Title = title;
            this.Description = description;
            this.WorkPackageId = WorkPackageId;
            this.Code = code;
            this.Type = type;

            return pstatus;
        }

        public IStatusGeneric CreateMDRStatus(string description, int commentStatusId,string folderName)
        {
            var pstatus = new StatusGenericHandler();
            var mdrStatus = MDRStatusHistory.CreateMDRStatus(description, commentStatusId, false, true
                ,folderName).Result;
            this.MDRStatusHistoryies.Add(mdrStatus);
            return pstatus;
        }

        public IStatusGeneric UpdateIsCompleted()
        {
            var pstatus = new StatusGenericHandler();

            this.IsCompleted = true;

            return pstatus;
        }
    }
}
