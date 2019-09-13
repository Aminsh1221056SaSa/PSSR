using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Logic.Activityes
{
    public class ActivityDocumentDto
    {
        public int Id { get;  set; }
        public string Description { get;  set; }
        public DateTime CreatedDate { get;  set; }
        public DateTime UpdatedDate { get;  set; }
        public string FilePath { get;  set; }
        public IFormFile File { get; set; }
        public String FileName { get; set; }
        public long ActivityId { get;  set; }
        public long? PunchId { get; set; }
    }
}
