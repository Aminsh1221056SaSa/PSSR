using Microsoft.EntityFrameworkCore;
using PSSR.DataLayer.EfCode;
using PSSR.DataLayer.QueryObjects;
using PSSR.ServiceLayer.ProjectServices;
using PSSR.ServiceLayer.SubSystemServices.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.SubSystemServices.Concrete
{
    public class ListProjectSubSystemService
    {
        private readonly EfCoreContext _context;

        public ListProjectSubSystemService(EfCoreContext context)
        {
            _context = context;
        }
        public async Task<ProjectSubSystemListDto> GetSubSystems(long id)
        {
            return await _context.ProjectSubSystems.Where(s=>s.Id==id).MapSubSystmeToDto().SingleOrDefaultAsync();
        }

        public async Task<List<ProjectSubSystemListDto>> GetSubSystemBySystem(long systemId)
        {
            return await _context.ProjectSubSystems.Where(s=>s.ProjectSystemId==systemId)
                .MapSubSystmeToDto().ToListAsync();
        }

        public async Task<IEnumerable<ProjectMapDto>> GetProjectSubSystems(Guid projectId)
        {
            var query = _context.ProjectSystems.Where(s => s.ProjectId == projectId);

            var items = await query.SelectMany(s => s.ProjectSubSystems)
                .Select(s => new ProjectMapDto
                {
                    ProjectId = projectId,
                    Id = s.Id,
                    Title = s.Code,
                    Description = s.Description,
                    HId=s.ProjectSystemId
                }).ToListAsync();

            return items;
        }

        public async Task<IQueryable<ProjectSubSystemListDto>> SortFilterPage
         (ProjectSubSystmeSortFilterPageOptions options,Guid projectId)
        {
            var systemIds =await _context.ProjectSystems.Where(s => s.ProjectId == projectId)
                .Select(s => s.Id).ToArrayAsync();

            var projectSubSystmes = _context.ProjectSubSystems.Where(s=>systemIds.Contains(s.ProjectSystemId))
                .AsNoTracking()
                .Where(sb=>sb.Code.StartsWith(options.QueryFilter) || sb.Description.Contains(options.QueryFilter)
                || string.IsNullOrWhiteSpace(options.QueryFilter))
                .MapSubSystmeToDto()
                .OrderProjectSubSystemBy(options.OrderByOptions)
                .FilterProjectSubSystemBy(options.FilterBy,
                               options.FilterValue);

            options.SetupRestOfDto(projectSubSystmes);

            return projectSubSystmes.Page(options.PageNum - 1,
                                   options.PageSize);
        }
    }
}
