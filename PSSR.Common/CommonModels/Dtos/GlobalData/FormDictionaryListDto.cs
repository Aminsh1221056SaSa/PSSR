using PSSR.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Common.FormDictionaryServices
{
    public class FormDictionaryListDto
    {
        public long Id { get; set; }
        public FormDictionaryType Type { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int WorkPackageId { get; set; }
        public string ActivityName { get; set; }
        public string FileName { get; set; }
        public string WrokPackageName { get; set; }
        public int Priority { get; set; }
        public float Mh { get; set; }
        public IEnumerable<string> Desciplines { get; set; }
        public IEnumerable<int> DesciplinesIds { get; set; }
    }

    public class FormDictionarySummaryDto
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public FormDictionaryType Type { get; set; }
        public string WrokPackageName { get; set; }
    }
}
