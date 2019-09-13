using PSSR.DataLayer.EfClasses.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.ProjectSystemServices.QueryObjects
{
    public static class ProjectSystmeListDtoSelect
    {
        public static IQueryable<ProjectSystemListDto>
           MapSystmeToDto(this IQueryable<ProjectSystem> projectSystmes)
        {
            return projectSystmes.Select(p => new ProjectSystemListDto
            {
                Code = p.Code,
                Description = p.Description,
                Id = p.Id,
                ProjectId = p.ProjectId,
                Type = p.Type
            });
        }
    }
}
