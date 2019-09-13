using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.ActivityServices
{
    public class ActivityDocumentListDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string CreatedDate { get; set; }
        public string FilePath { get; set; }
        public String FileName { get; set; }
        public long ActivityId { get; set; }
        public long? PunchId { get; set; }
        public string PunchCode { get; set; }
    }
}
