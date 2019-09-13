using PSSR.ServiceLayer.MDRDocumentServices.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.MDRDocumentServices
{
    public class MDRDocumentListCombinedDto
    {
        public MDRDocumentListCombinedDto(MDRDocumentSortFilterPageOptions sortFilterPageData,
               IEnumerable<MDRDocumentListDto> mdrDicList)
        {
            SortFilterPageData = sortFilterPageData;
            MDRDocumentlineList = mdrDicList;
        }
        public MDRDocumentSortFilterPageOptions SortFilterPageData { get; private set; }

        public IEnumerable<MDRDocumentListDto> MDRDocumentlineList { get; private set; }
    }
}
