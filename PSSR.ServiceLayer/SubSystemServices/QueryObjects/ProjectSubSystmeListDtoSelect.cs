using PSSR.DataLayer.EfClasses.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.SubSystemServices.QueryObjects
{
    public static class ProjectSubSystmeListDtoSelect
    {
        public static IQueryable<ProjectSubSystemListDto>
          MapSubSystmeToDto(this IQueryable<ProjectSubSystem> projectSubSystmes)
        {
            return projectSubSystmes.Select(p => new ProjectSubSystemListDto
            {
                PriorityNo = p.PriorityNo,
                Code = p.Code,
                SubPriorityNo = p.SubPriorityNo,
                Description = p.Description,
                Id = p.Id,
                ProjectSystemId = p.ProjectSystemId,
                SystemCode = p.ProjectSystem.Code
            });
        }
    }
}
