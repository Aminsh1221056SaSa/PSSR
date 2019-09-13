using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.MDRStatuses
{
    public class MDRStatusListCombinedDto
    {
        public MDRStatusListCombinedDto(MDRStatusSortFilterPageOptions sortFilterPageData,
               IEnumerable<MDRStatusListDto> mdrDicList)
        {
            SortFilterPageData = sortFilterPageData;
            MDRStatuslineList = mdrDicList;
        }
        public MDRStatusSortFilterPageOptions SortFilterPageData { get; private set; }

        public IEnumerable<MDRStatusListDto> MDRStatuslineList { get; private set; }
    }
}
