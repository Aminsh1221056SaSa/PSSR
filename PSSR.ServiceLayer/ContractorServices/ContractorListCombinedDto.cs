using PSSR.Common.ContractorServices;
using PSSR.ServiceLayer.ContractorServices.Concrete;
using System.Collections.Generic;

namespace PSSR.ServiceLayer.ContractorServices
{
    public class ContractorListCombinedDto
    {
        public ContractorListCombinedDto(ContractorSortFilterPageOptions sortFilterPageData, 
            IEnumerable<ContractorListDto> contractorList)
        {
            SortFilterPageData = sortFilterPageData;
            ContractorList = contractorList;
        }

        public ContractorSortFilterPageOptions SortFilterPageData { get; private set; }
        public IEnumerable<ContractorListDto> ContractorList { get; private set; }
    }
}
