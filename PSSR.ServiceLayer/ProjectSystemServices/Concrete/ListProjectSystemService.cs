using Microsoft.EntityFrameworkCore;
using PSSR.DataLayer.EfCode;
using System;
using System.Collections.Generic;
using System.Linq;
using PSSR.ServiceLayer.ProjectSystemServices.QueryObjects;
using PSSR.DataLayer.QueryObjects;
using System.Threading.Tasks;
using PSSR.ServiceLayer.Utils.ChartsDto;
using PSSR.ServiceLayer.ProjectServices;
using PSSR.ServiceLayer.Utils;

namespace PSSR.ServiceLayer.ProjectSystemServices.Concrete
{
    public class ListProjectSystemService
    {
        private readonly EfCoreContext _context;

        public ListProjectSystemService(EfCoreContext context)
        {
            _context = context;
        }

        public async Task<ProjectSystemListDto> GetSystem(int id)
        {
            return await _context.ProjectSystems.Where(s => s.Id == id)
                .MapSystmeToDto().SingleOrDefaultAsync();
        }

        public async Task<List<DropdownTuple>> GetProjectSystemsSummary(Guid projectId)
        {
            var items = await _context.Projects.Where(s => s.Id == projectId).SelectMany(s => s.ProjectSystems)
                .Select(s => new DropdownTuple
                {
                   Text=s.Code,
                   Value=s.Id.ToString()
                }).ToListAsync();

            return items;
        }

        public async Task<List<ProjectMapDto>> GetProjectSystems(Guid projectId)
        {
            var items = await _context.Projects.Where(s => s.Id == projectId).SelectMany(s => s.ProjectSystems)
                .Select(s => new ProjectMapDto
                {
                    ProjectId = s.ProjectId,
                    Id = s.Id,
                    Title = s.Code,
                    Description = s.Description,
      
                }).ToListAsync();

            return items;
        }

        public IQueryable<ProjectSystemListDto> SortFilterPage
         (ProjectSystmeSortFilterPageOptions options,Guid projectId)
        {
            var projectSystmes = _context.ProjectSystems.Where(s=>s.ProjectId==projectId)
                .AsNoTracking().
                 Where(sb => sb.Code.StartsWith(options.QueryFilter) || sb.Description.Contains(options.QueryFilter)
                || string.IsNullOrWhiteSpace(options.QueryFilter))
                .MapSystmeToDto()
                .OrderProjectSystemBy(options.OrderByOptions)
                .FilterProjectSystemBy(options.FilterBy,
                               options.FilterValue);

            options.SetupRestOfDto(projectSystmes);

            return projectSystmes.Page(options.PageNum - 1,
                                   options.PageSize);
        }

        public async Task<BarChartDto> getActivityStatusBySystem()
        {
            var systems = await _context.ProjectSubSystems.OrderBy(s => s.Id).Select(s =>$"{s.Code}({s.Description})")
                .ToListAsync();

            var allActivity = await _context.Activites.Include(s=>s.SubSystem).ToListAsync();

            var gstatus = allActivity.GroupBy(s => s.Status);

            var viewModel = new BarChartDto();
            viewModel.Desciplines = systems;

            foreach (var gbyd in gstatus)
            {
                BarChartDetails<string, object> aSeries = new BarChartDetails<string, object>();
                aSeries["name"] = gbyd.Key.ToString();
                var lstDate = new List<int>();

                if (gbyd.Key == Common.ActivityStatus.Done)
                {
                    aSeries["color"] = $"#{Common.ActivityStatusColor.A3db08}";
                }
                else if (gbyd.Key == Common.ActivityStatus.NotStarted)
                {
                    aSeries["color"] = $"#{Common.ActivityStatusColor.FF530D}";
                }
                else if (gbyd.Key == Common.ActivityStatus.Ongoing)
                {
                    aSeries["color"] = $"#{Common.ActivityStatusColor.E8EC26}";
                }
                else if (gbyd.Key == Common.ActivityStatus.Reject)
                {
                    aSeries["color"] = $"#{Common.ActivityStatusColor.DE1515}";
                }

                var gByStatus = gbyd.OrderBy(s => s.SubSystem.ProjectSystemId).GroupBy(s => s.SubSystem.ProjectSystemId);

                foreach (var item in gByStatus)
                {
                    lstDate.Add(item.Count());
                }
                aSeries["data"] = lstDate;
                viewModel.Values.Add(aSeries);
            }
            return viewModel;
        }

        public async Task<BarChartDto> getActivityConditionBySystem()
        {
            var systems = await _context.ProjectSubSystems.OrderBy(s => s.Id).Select(s => $"{s.Code}({s.Description})")
               .ToListAsync();

            var allActivity = await _context.Activites.Where(s =>s.Condition != Common.ActivityCondition.Normal).Include(s => s.SubSystem)
                .ToListAsync();

            var gstatus = allActivity.GroupBy(s => s.Condition);

            var viewModel = new BarChartDto();
            viewModel.Desciplines = systems;

            foreach (var gbyd in gstatus)
            {
                BarChartDetails<string, object> aSeries = new BarChartDetails<string, object>();
                aSeries["name"] = gbyd.Key.ToString();
                var lstDate = new List<int>();

                var gByStatus = gbyd.OrderBy(s => s.SubSystem.ProjectSystemId).GroupBy(s => s.SubSystem.ProjectSystemId);

                foreach (var item in gByStatus)
                {
                    lstDate.Add(item.Count());
                }
                aSeries["data"] = lstDate;
                viewModel.Values.Add(aSeries);
            }
            return viewModel;
        }

        public async Task<PieChartsListDto> getActivityCounterBySystem()
        {
            var systems = await _context.ProjectSystems.OrderBy(s => s.Id)
                .ToDictionaryAsync(x => x.Id, s => s.Description);

            var allActivity = await _context.Activites.Include(s=>s.SubSystem).ToListAsync();

            var gGroup = allActivity.GroupBy(s => s.SubSystem.ProjectSystemId);

            var viewModel = new PieChartsListDto();


            foreach (var gbyd in gGroup)
            {
                var desc = systems[gbyd.Key];
                var item = new PieChartDto
                {
                    Name = desc,
                    Y = gbyd.Count()
                };

                viewModel.Values.Add(item);
            }

            if (viewModel.Values.Any())
            {
                var maxItem = viewModel.Values.OrderByDescending(i => i.Y).First();
                maxItem.Selected = true;
                maxItem.Sliced = true;
            }

            return viewModel;
        }

        public async Task<BarChartDto> getActivityTaskDoneBySystem()
        {
            var viewModel = new BarChartDto();

            var systems = await _context.ProjectSystems.OrderBy(s => s.Id)
               .ToDictionaryAsync(x => x.Id, s => $"{s.Code}({s.Description})");

            var allActivity = await _context.Activites.Where(s =>s.Status == Common.ActivityStatus.Done
            && s.UpdatedDate >= DateTime.Now.AddDays(-30))
            .GroupBy(s => new { s.SubSystem.ProjectSystemId, s.UpdatedDate.Date }).ToListAsync();

            List<List<object>> lstItms = null;
            BarChartDetails<string, object> aSeries = null;

            allActivity.ForEach(item =>
            {
                var desc = systems[item.Key.ProjectSystemId];

                if (viewModel.Values.Any(s => s.ContainsValue(desc)))
                {
                    lstItms.Add(new List<object> { Convert.ToInt64((item.Key.Date -
                        new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds), item.Count() });
                }
                else
                {
                    aSeries = new BarChartDetails<string, object>();
                    lstItms = new List<List<object>>();
                    aSeries["name"] = desc;
                    lstItms.Add(new List<object> { Convert.ToInt64((item.Key.Date -
                        new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds), item.Count() });

                    aSeries["data"] = lstItms;
                    viewModel.Values.Add(aSeries);
                }
            });

            return viewModel;
        }
    }
}
