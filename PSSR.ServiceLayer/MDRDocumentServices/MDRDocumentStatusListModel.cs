using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.MDRDocumentServices
{
    public class MDRDocumentStatusListModel
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public int StatusId { get; set; }
        public long MdrDocumentId { get; set; }
        public string StatusName { get; set; }
        public string FilePath { get; set; }
        public float WF { get; set; }
        public bool IsIFR { get; set; }
        public bool IsContractorConfirm { get; set; }
    }
}
