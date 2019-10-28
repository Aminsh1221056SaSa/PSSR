using PSSR.Common.PersonService;
using PSSR.Common.ProjectServices;
using PSSR.ServiceLayer.PersonService.Concrete;
using PSSR.ServiceLayer.ProjectServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.PersonService
{
    public class PersonListCombinedDto
    {
        public PersonListCombinedDto(PersonSortFilterPageOptions sortFilterPageData,
               IEnumerable<PersonListDto> personList, List<ProjectSummaryListDto> projects)
        {
            SortFilterPageData = sortFilterPageData;
            PersonList = personList;
            Projects = projects;
        }
        public PersonSortFilterPageOptions SortFilterPageData { get; private set; }
        public IEnumerable<PersonListDto> PersonList { get; private set; }
        public IEnumerable<ProjectSummaryListDto> Projects { get; private set; }
    }
}
