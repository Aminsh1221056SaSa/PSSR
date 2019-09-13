
using Microsoft.AspNetCore.Mvc.Rendering;
using PSSR.ServiceLayer.ProjectServices;
using PSSR.ServiceLayer.SubSystemServices.Concrete;
using System.Collections.Generic;

namespace PSSR.ServiceLayer.SubSystemServices
{
    public class ProjectSubSustemListCombinedDto
    {
        public ProjectSubSustemListCombinedDto(ProjectSubSystmeSortFilterPageOptions sortFilterPageData, IEnumerable<ProjectSubSystemListDto> projectSubSystmes,
            IEnumerable<ProjectMapDto> projectsystems)
        {
            SortFilterPageData = sortFilterPageData;
            ProjectSubSystemist = projectSubSystmes;
            SystemList = new SelectList(projectsystems, "Id", "Title");
        }

        public ProjectSubSystmeSortFilterPageOptions SortFilterPageData { get; private set; }

        public IEnumerable<ProjectSubSystemListDto> ProjectSubSystemist { get; private set; }
        public SelectList SystemList { get; private set; }
    }
}
