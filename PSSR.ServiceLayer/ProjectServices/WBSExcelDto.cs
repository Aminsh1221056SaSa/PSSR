using PSSR.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.ProjectServices
{
    public class WBSExcelDto
    {
        public string Name { get; set; }
        public string WBSCode { get; set; }
        public float WF { get; set; }
        public float Progress { get; set; }
        public int ParentLevel { get; set; }
        public int ActivityCount { get; set; }
        public int LastCounter { get; set; }
        public WBSType Type { get; set; }
    }

    public class ExcelGroupedItems
    {
        public ExcelGroupedItems()
        {
            Groups = new List<KeyValuePair<int, Tuple<int, int>>>();
            Items = new List<WBSExcelDto>();
        }

        public List<WBSExcelDto> Items { get; set; }
        public List<KeyValuePair<int,Tuple<int,int>>> Groups { get; set; }
    }
}
