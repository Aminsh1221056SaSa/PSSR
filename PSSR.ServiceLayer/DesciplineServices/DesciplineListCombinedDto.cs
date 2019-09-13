using PSSR.ServiceLayer.DesciplineServices.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.DesciplineServices
{
    public class DesciplineListCombinedDto
    {
        public DesciplineListCombinedDto(DesciplineSortFilterPageOptions sortFilterPageData, IEnumerable<DesciplineListDto> desciplineList)
        {
            SortFilterPageData = sortFilterPageData;
            DesciplineList = desciplineList;
        }

        public DesciplineSortFilterPageOptions SortFilterPageData { get; private set; }

        public IEnumerable<DesciplineListDto> DesciplineList { get; private set; }
    }
}
