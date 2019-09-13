using PSSR.ServiceLayer.PunchTypeServices.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.PunchTypeServices
{
    public class PunchTypeListCombinedDto
    {
        public PunchTypeListCombinedDto(PunchTypeSortFilterPageOptions sortFilterPageData, IEnumerable<PunchTypeListDto> punchTypesList)
        {
            SortFilterPageData = sortFilterPageData;
            PunchTypeList = punchTypesList;
        }

        public PunchTypeSortFilterPageOptions SortFilterPageData { get; private set; }

        public IEnumerable<PunchTypeListDto> PunchTypeList { get; private set; }
    }
}
