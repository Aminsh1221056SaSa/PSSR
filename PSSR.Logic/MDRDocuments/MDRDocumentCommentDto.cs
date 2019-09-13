using Microsoft.AspNetCore.Http;
using System;

namespace PSSR.Logic.MDRDocuments
{
    public class MDRDocumentCommentDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public long MDRDocumentId { get; set; }
        public string FilePath { get; set; }
        public IFormFile File { get; set; }
    }
}
