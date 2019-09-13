using PSSR.ServiceLayer.ContractorServices;
using PSSR.ServiceLayer.ProjectServices.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.ProjectServices
{
    public class ProjectListCombinedDto
    {
        public ProjectListCombinedDto(ProjectSortFilterPageOptions sortFilterPageData, IEnumerable<ProjectListDto> projects
            , IEnumerable<ContractorListDto> contractors)
        {
            SortFilterPageData = sortFilterPageData;
            ProjectList = projects;
            Contractors = contractors;
        }

        public ProjectSortFilterPageOptions SortFilterPageData { get; private set; }
        public IEnumerable<ProjectListDto> ProjectList { get; private set; }
        public IEnumerable<ContractorListDto> Contractors { get; private set; }
    }
}
