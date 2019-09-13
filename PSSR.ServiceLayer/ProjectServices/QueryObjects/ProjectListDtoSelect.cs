using PSSR.DataLayer.EfClasses.Projects;
using System.Linq;

namespace PSSR.ServiceLayer.ProjectServices.QueryObjects
{
    public static class ProjectListDtoSelect
    {
        public static IQueryable<ProjectListDto> MapProjectToDto(this IQueryable<Project> projects)
        {
            return projects.Select(item => new ProjectListDto
            {
                Description = item.Description,
                EndDate = item.EndDate != null ? item.EndDate.Value.ToString("d") : "",
                StartDate = item.StartDate != null ? item.StartDate.Value.ToString("d") : "",
                ContractorId = item.ContractorId,
                Id = item.Id,
                Type=item.Type,
                ContractorName=item.Contractor.Name
            });
        }
    }
}
