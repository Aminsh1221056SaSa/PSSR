using Microsoft.AspNetCore.Http;
using PSSR.Common;
using System;

namespace PSSR.Logic.FormDictionaries
{
    public class FormDictionaryDto
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public FormDictionaryType Type { get; set; }
        public int WorkPackageId { get; set; }
        public string ActivityName { get; set; }
        public IFormFile File { get; set; }
        public String FileName { get; set; }
        public int[] AvailableDesciplines { get; set; }
        public int Priority { get; set; }
        public float Mh { get; set; }
    }
}
