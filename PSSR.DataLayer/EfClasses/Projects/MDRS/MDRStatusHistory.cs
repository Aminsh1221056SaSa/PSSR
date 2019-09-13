using BskaGenericCoreLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.DataLayer.EfClasses.Projects.MDRS
{
    public class MDRStatusHistory : IAuditTracker
    {
        public long Id { get; private set; }
        public string Description { get; private set; }
        public string FolderName { get; private set; }
        public DateTime CreatedDate { get; internal set; }
        public DateTime UpdatedDate { get; internal set; }
        public bool IsIFR { get; set; }
        public bool IsContractorConfirm { get; set; }
        //-----------------------------------------
        //Relationships
        public long MDRDocumentId { get; private set; }
        public MDRDocument MDRDocument { get; private set; }
        public int MdrStatusId { get; private set; }
        public MDRStatus HStatus { get; private set; }
        private MDRStatusHistory() { }
        public MDRStatusHistory(string description, int statusId)
        {
            this.Description = description;
            this.MdrStatusId = statusId;
        }

        public static IStatusGeneric<MDRStatusHistory> CreateMDRStatus(string description,int statusId,
            bool isIfr,bool contractorConfirm, string folderName)
        {
            var pstatus = new StatusGenericHandler<MDRStatusHistory>();

            var newMDRDoc = new MDRStatusHistory
            {
                Description = description,
                MdrStatusId = statusId,
                IsIFR=isIfr,
                IsContractorConfirm=contractorConfirm,
                FolderName=folderName
            };

            pstatus.Result = newMDRDoc;
            return pstatus;
        }

        public IStatusGeneric ConfirmContractor()
        {
            var pstatus = new StatusGenericHandler();
            this.IsContractorConfirm = true;
            return pstatus;
        }
    }
}
