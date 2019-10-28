using PSSR.DataLayer.EfCode;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using PSSR.ServiceLayer.ProjectServices.QueryObjects;
using PSSR.DataLayer.QueryObjects;
using PSSR.Common.ProjectServices;

namespace PSSR.ServiceLayer.ProjectServices.Concrete
{
    public class ListProjectService
    {
        private readonly EfCoreContext _context;

        public ListProjectService(EfCoreContext context)
        {
            _context = context;
        }

        public ProjectListDto GetProject(Guid projectId)
        {
            return _context.Projects.Where(s=>s.Id==projectId).Select(item => new ProjectListDto
            {
                Description = item.Description,
                EndDate = item.EndDate != null ? item.EndDate.Value.ToString("d") : "",
                StartDate = item.StartDate != null ? item.StartDate.Value.ToString("d") : "",
                ContractorId = item.ContractorId,
                Id = item.Id,
                Type = item.Type
            }).FirstOrDefault();
        }

        public async Task<List<ProjectSummaryListDto>> GetProjectSummary()
        {
            return await _context.Projects.Select(p => new ProjectSummaryListDto
            {
                Id=p.Id,
                Name=p.Description,
                ContractorName=p.Contractor.Name,
                Type=p.Type
            }).ToListAsync();
        }

        public async Task<ProjectListDto> GetProjectDetails(Guid projectId)
        {
            return await _context.Projects.Where(s=>s.Id== projectId).Select(item => new ProjectListDto
            {
                Description = item.Description,
                EndDate = item.EndDate != null ? item.EndDate.Value.ToString("MM/dd/yyyy") : "",
                StartDate = item.StartDate != null ? item.StartDate.Value.ToString("MM/dd/yyyy") : "",
                ContractorId = item.ContractorId,
                Id = item.Id,
                Type = item.Type
            }).SingleAsync();
        }

        public async Task<IEnumerable<ProjectListDto>> GetProjectsForAdmin()
        {
            return await _context.Projects.Select(item => new ProjectListDto
            {
                Description = item.Description,
                EndDate = item.EndDate != null ? item.EndDate.Value.ToString("d") : "",
                StartDate = item.StartDate != null ? item.StartDate.Value.ToString("d") : "",
                ContractorId = item.ContractorId,
                Id = item.Id,
                Type = item.Type
            }).ToListAsync();
        }

        public async Task<IEnumerable<ProjectListDto>> GetallCurrentUserprojects(int personId)
        {
            return await _context.Projects.Where(s=>s.AgentsLink.Any(ag=>ag.PersonId==personId)).Select(item => new ProjectListDto
            {
                Description = item.Description,
                EndDate = item.EndDate != null ? item.EndDate.Value.ToString("d") : "",
                StartDate = item.StartDate != null ? item.StartDate.Value.ToString("d") : "",
                ContractorId = item.ContractorId,
                Id = item.Id,
                Type=item.Type
            }).ToListAsync();
        }

        public IQueryable<ProjectListDto> SortFilterPage
         (ProjectSortFilterPageOptions options)
        {
            var projectSystmes = _context.Projects
                .AsNoTracking().
                 Where(sb => sb.Description.StartsWith(options.QueryFilter)
                || string.IsNullOrWhiteSpace(options.QueryFilter))
                .MapProjectToDto()
                .OrderProjectBy(options.OrderByOptions)
                .FilterProjectBy(options.FilterBy,
                               options.FilterValue);

            options.SetupRestOfDto(projectSystmes);

            return projectSystmes.Page(options.PageNum - 1,
                                   options.PageSize);
        }


    }
}
