using Microsoft.AspNetCore.Mvc.Rendering;
using PSSR.Common.DesciplineServices;
using PSSR.Common.FormDictionaryServices;
using PSSR.Common.RoadMapServices;
using PSSR.ServiceLayer.DesciplineServices;
using PSSR.ServiceLayer.FormDictionaryServices.Concrete;
using PSSR.ServiceLayer.RoadMapServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.FormDictionaryServices
{
    public class FormDictionaryListCombinedDto
    {
        public FormDictionaryListCombinedDto() { }

        public FormDictionaryListCombinedDto(FormDictionarySortFilterPageOptions sortFilterPageData,
            IEnumerable<FormDictionaryListDto> formDicList,IEnumerable<DesciplineListDto> desciplines
            , IEnumerable<WorkPackageListDto> workPackages)
        {
            SortFilterPageData = sortFilterPageData;
            FormDictioanrylineList = formDicList;
            DesciplineList = desciplines;
            WorkPackageList =workPackages;
        }
        public FormDictionarySortFilterPageOptions SortFilterPageData { get;  set; }
        
        public IEnumerable<FormDictionaryListDto> FormDictioanrylineList { get;  set; }
        public IEnumerable<DesciplineListDto> DesciplineList { get;  set; }
        public IEnumerable<WorkPackageListDto> WorkPackageList { get;  set; }
    }
}
