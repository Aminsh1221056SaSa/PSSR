using PSSR.ServiceLayer.PunchServices.Concrete;
using System.Collections.Generic;

namespace PSSR.ServiceLayer.PunchServices
{
    public class PunchListCombinedDto
    {
        public PunchListCombinedDto(PunchSortFilterPageOptions sortFilterPageData, IEnumerable<PunchListDto> punchList)
        {
            SortFilterPageData = sortFilterPageData;
            PunchList = punchList;
        }

        public PunchSortFilterPageOptions SortFilterPageData { get; private set; }

        public IEnumerable<PunchListDto> PunchList { get; private set; }
    }
}
