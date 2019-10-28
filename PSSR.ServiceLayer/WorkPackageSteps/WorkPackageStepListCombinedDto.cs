using PSSR.Common.WorkPackageSteps;
using PSSR.ServiceLayer.WorkPackageSteps.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.WorkPackageSteps
{
    public class WorkPackageStepListCombinedDto
    {
        public WorkPackageStepListCombinedDto(WorkPackageStepSortFilterPageOptions sortFilterPageData,
            IEnumerable<WorkPackageStepListDto> stepList)
        {
            SortFilterPageData = sortFilterPageData;
            WorkStepList = stepList;
        }

        public WorkPackageStepSortFilterPageOptions SortFilterPageData { get; private set; }

        public IEnumerable<WorkPackageStepListDto> WorkStepList { get; private set; }
    }
}
