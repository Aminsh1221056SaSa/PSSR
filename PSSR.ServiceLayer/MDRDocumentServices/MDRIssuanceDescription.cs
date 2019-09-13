using PSSR.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.MDRDocumentServices
{
    public class MDRIssuanceDescription
    {
        public long Id { get; set; }
        public string LastStatus { get; set; }
        public int UnclearComment { get; set; }
        public string LastCommentDate { get; set; }
        public string NextStatusDescription { get; set; }
        public int NextStatusId { get; set; }
        public MDRDocumentType Type { get; set; }
        public string Description { get; set; }
    }
}
