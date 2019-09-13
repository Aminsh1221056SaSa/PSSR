using PSSR.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.MDRDocumentServices
{
    public class MDRDocumentSummaryDto
    {
        public long Id { get;  set; }
        public string Code { get; set; }
        public string Title { get;  set; }
        public string Description { get;  set; }
        public int WorkPackageId { get;  set; }
        public string LastIssuanceDate { get; set; }
        public string CreateDate { get; set; }
        public string CurrentStatus { get; set; }
    }
}
