using PSSR.ServiceLayer.ProjectSystemServices.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.ProjectSystemServices
{
    public class ProjectSystemListCombinedDto
    {
        public ProjectSystemListCombinedDto(ProjectSystmeSortFilterPageOptions sortFilterPageData,IEnumerable<ProjectSystemListDto> projectSystmes)
        {
            SortFilterPageData = sortFilterPageData;
            ProjectSystemist = projectSystmes;
        }
        public ProjectSystmeSortFilterPageOptions SortFilterPageData { get; private set; }

        public IEnumerable<ProjectSystemListDto> ProjectSystemist { get; private set; }
    }
}
