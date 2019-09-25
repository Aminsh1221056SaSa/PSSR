using PSSR.DataLayer.EfCode;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using PSSR.ServiceLayer.ProjectServices.QueryObjects;
using PSSR.DataLayer.QueryObjects;
using PSSR.ServiceLayer.Utils.ChartsDto;

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

        public async Task<ProjectListDto> GetProjectFormatedDate(Guid projectId)
        {
            var item =await _context.Projects.Where(s=>s.Id==projectId).FirstOrDefaultAsync();
            if (item == null) return null;

            var rItem = new ProjectListDto
            {
                Description = item.Description,
                EndDate = item.EndDate != null ? item.EndDate.Value.ToString("dddd, dd MMMM yyyy") : "",
                StartDate = item.StartDate != null ? item.StartDate.Value.ToString("dddd, dd MMMM yyyy") : "",
                ContractorId = item.ContractorId,
                Id = item.Id,
                Type=item.Type
            };

            var systemsIds = await _context.ProjectSystems.Where(s => s.ProjectId == projectId)
                .Select(s=>s.Id).ToArrayAsync();
            var subsystemIds = await _context.ProjectSubSystems.Where(s => systemsIds.Contains(s.ProjectSystemId))
                .Select(s => s.Id).ToArrayAsync();

            
            rItem.SystemsCount =systemsIds.Count();
            rItem.SubSystemsCount = subsystemIds.Count();
            
            rItem.ActivitysCount = await _context.Activites.Where(s=>subsystemIds.Contains(s.SubsytemId)).CountAsync();
            
            if (item.StartDate.HasValue)
            {
                rItem.ElapsedDate = (DateTime.Now - item.StartDate.Value).Days;
            }

            if (item.EndDate.HasValue)
            {
                if (item.EndDate.Value > DateTime.Now)
                {
                    rItem.RemainedDate = (item.EndDate.Value -DateTime.Now).Days;
                }
            }
            return rItem;
        }

        public async Task<BarChartDto> GetAllActivityTaskDonePerDayForUser(int personId)
        {
            var pids = await _context.Projects.Where(s => s.AgentsLink.Any(ag => ag.PersonId == personId))
                 .Select(s => s.Id).ToArrayAsync();

            var subIds = await _context.ProjectSubSystems.Where(s => pids.Contains(s.ProjectSystem.ProjectId))
                .Select(s => s.Id).ToArrayAsync();

            var viewModel = new BarChartDto();

            var allActivity = await _context.Activites.Where(s => subIds.Contains(s.SubsytemId)
            && s.Status == Common.ActivityStatus.Done && s.ActualEndDate.HasValue)
            .GroupBy(s => new { s.ActualEndDate.Value.Date }).ToListAsync();

            List<List<object>> lstItms = null;
            BarChartDetails<string, object> aSeries = null;
            aSeries = new BarChartDetails<string, object>();
            lstItms = new List<List<object>>();
            aSeries["name"] = "Done Activity";

            allActivity.ForEach(item =>
            {
                lstItms.Add(new List<object> { Convert.ToInt64((item.Key.Date -
                    new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds), item.Count() });
            });

            aSeries["data"] = lstItms;
            viewModel.Values.Add(aSeries);

            return viewModel;
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
