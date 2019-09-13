using PSSR.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.MDRDocumentServices
{
    public class MDRDocumentCommentListDto
    {
        public bool IsClear { get; set; }
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreateDate { get; set; }
        public long MDRDocumentId { get; set; }
        public string Code { get; set; }
        public bool HasFilePath { get; set; }
    }
}
