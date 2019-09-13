using PSSR.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.MDRDocumentServices
{
    public class MDRDocumentListDto
    {
        public long Id { get; set; }
        public bool IsCompleted { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int WorkPackageId { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string LastIssuanceDate { get; set; }
        public string WBSName { get; set; }
        public string Code { get; set; }
        public int LastStatusId { get; set; }
        public string LastStatusName { get; set; }
        public int CommentCount { get; set; }
        public float Progress { get; set; }
        public bool HaveUnclearComment { get; set; }
        public MDRDocumentType Type { get; set; }
    }

    public class MDRSummaryDto
    {
        public int WorkPackageId { get; set; }
        public string Name { get; set; }
        public int Total { get; set; }
        public int Done { get; set; }
        public string Link { get; set; }
    }
}
