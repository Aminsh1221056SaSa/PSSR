using PSSR.ServiceLayer.PunchCategoryServices.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.PunchCategoryServices
{
    public class PunchCategoryListCombinedDto
    {
        public PunchCategoryListCombinedDto(PunchCategorySortFilterPageOptions sortFilterPageData, IEnumerable<PunchCategoryListDto> punchTypesList)
        {
            SortFilterPageData = sortFilterPageData;
            PunchTypeList = punchTypesList;
        }

        public PunchCategorySortFilterPageOptions SortFilterPageData { get; private set; }

        public IEnumerable<PunchCategoryListDto> PunchTypeList { get; private set; }
    }
}
