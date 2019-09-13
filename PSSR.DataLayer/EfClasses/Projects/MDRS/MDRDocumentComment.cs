using BskaGenericCoreLib;
using System;

namespace PSSR.DataLayer.EfClasses.Projects.MDRS
{
    public class MDRDocumentComment : IAuditTracker
    {
        public long Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool IsClear { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedDate { get; internal set; }
        public DateTime UpdatedDate { get; internal set; }
        //-----------------------------------------
        //Relationships
        public long MDRDocumentId { get; private set; }
        public MDRDocument MDRDocument { get; private set; }

        private MDRDocumentComment() { }
        public MDRDocumentComment(string title, string description, long mdrDocId)
        {
            this.Title = title;
            this.Description = description;
            this.MDRDocumentId = mdrDocId;
        }

        public static IStatusGeneric<MDRDocumentComment> CreateMDRDocument(string title, string description, 
            long mdrDocId,string filePath)
        {
            var pstatus = new StatusGenericHandler<MDRDocumentComment>();

            var newMDRDoc = new MDRDocumentComment
            {
                Description = description,
                MDRDocumentId= mdrDocId,
                Title = title,
                FilePath=filePath
            };

            pstatus.Result = newMDRDoc;
            return pstatus;
        }

        public IStatusGeneric UpdateMDRDocument(string title, string description, long mdrDocId)
        {
            var pstatus = new StatusGenericHandler();

            this.Title = title;
            this.Description = description;
            this.MDRDocumentId = mdrDocId;

            return pstatus;
        }

        public IStatusGeneric ClearComment()
        {
            var pstatus = new StatusGenericHandler();
            this.IsClear = true;
            return pstatus;
        }
    }
}
