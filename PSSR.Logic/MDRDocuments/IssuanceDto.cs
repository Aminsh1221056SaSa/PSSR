using Microsoft.AspNetCore.Http;
using PSSR.Common;
using System;
using System.Collections.Generic;

namespace PSSR.Logic.MDRDocuments
{
    public class IssuanceDto
    {
        public long MdrId { get; set; }
        public int StatusId { get; set; }
        public string NextStatus { get; set; }
        public string Description { get; set; }
        public MDRDocumentType Type { get; set; }
        public bool IsConfirmContractor { get; set; }
        public string Code { get; set; }
        public Guid ProjectId { get; set; }
        public String FolderName { get; set; }
        public List<IFormFile> NativeFiles { get; set; }
        public List<IFormFile> PDFFiles { get; set; }
        public List<IFormFile> AttachMentFiles { get; set; }
    }
}
