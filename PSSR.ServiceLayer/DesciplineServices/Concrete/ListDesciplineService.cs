using Microsoft.EntityFrameworkCore;
using PSSR.Common;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DataLayer.EfCode;
using PSSR.DataLayer.QueryObjects;
using PSSR.ServiceLayer.DesciplineServices.QueryObjects;
using PSSR.ServiceLayer.Utils.ChartsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.DesciplineServices.Concrete
{
    public class ListDesciplineService
    {
        private readonly EfCoreContext _context;

        public ListDesciplineService(EfCoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetNames()
        {
            var items = await _context.Desciplines.OrderBy(s=>s.Id)
                .Select(s => s.Name).ToListAsync();
            return items;
        }

        public async Task<DesciplineListDto> GetDescipline(int id)
        {
            var item= await _context.FindAsync<Descipline>(id);
            return new DesciplineListDto
            {
                CreatedDate=item.CreatedDate,
                Id=item.Id,
                Description=item.Description,
                Title=item.Name
            };
        }

        public IQueryable<DesciplineListDto> SortFilterPage
           (DesciplineSortFilterPageOptions options)
        {
            var desciplineQuery = _context.Desciplines
                .AsNoTracking()
                 .Where(item =>item.Description.Contains(options.QueryFilter) || item.Name.Contains(options.QueryFilter)
                 || string.IsNullOrWhiteSpace(options.QueryFilter))
                .FilterDesciplineBy(options.FilterBy,
                               options.FilterValue)
                .MapBookToDto()
                .OrderDesciplineBy(options.OrderByOptions);

            options.SetupRestOfDto(desciplineQuery);

            return desciplineQuery.Page(options.PageNum - 1,
                                   options.PageSize);
        }

        public async Task<List<DesciplineListDto>> GetAllDesciplines()
        {
           return await _context.Desciplines.MapBookToDto().ToListAsync();
        }

        public async Task<Dictionary<string, IEnumerable<Tuple<long, string,int>>>> GetAllDesciplinesIdToFormCode()
        {
            return await _context.Desciplines.Include(x => x.FormDictionaryLink).ThenInclude(x=>x.FormDictionary)
                .ToDictionaryAsync(x => x.Name.ToUpper(), x => x.FormDictionaryLink.Select(s => new
                     Tuple<long, string,int>(s.FormDictionary.Id, s.FormDictionary.Code.ToUpper(),x.Id)));
        }

        //reports

        public async Task<BarChartDto> getActivityStatusByDesciplineForWorkPackage(int workPackageId,
            int locationId,bool total,Guid projectId)
        {
            var desciplens = await _context.Desciplines.ToListAsync();
            var subsystemIds = await _context.ProjectSystems.Where(s => s.ProjectId == projectId)
               .SelectMany(s => s.ProjectSubSystems)
               .Select(s => s.Id).ToArrayAsync();

            var allActivity = new List<Activity>();

            if(locationId<=0)
            {
                allActivity = await _context.Activites.Where(s => subsystemIds.Contains(s.SubsytemId) && s.WorkPackageId == workPackageId)
               .ToListAsync();
            }
            else
            {
                allActivity = await _context.Activites.Where(s => subsystemIds.Contains(s.SubsytemId)
                && s.WorkPackageId == workPackageId
                && s.LocationId==locationId)
               .ToListAsync();
            }

            var availableDes = allActivity.OrderBy(s => s.Id).Select(s => s.DesciplineId).ToArray();

            var viewModel = new BarChartDto();
            viewModel.Desciplines = desciplens.Where(s => availableDes.Contains(s.Id))
                .OrderBy(s => s.Id).Select(s => s.Name).ToList();

            if(total)
            {
                var gstatus = allActivity.Where(s => s.Status == ActivityStatus.Done).ToList();
                BarChartDetails<string, object> aSeriesTotal = new BarChartDetails<string, object>();
                aSeriesTotal["name"] = "Total";
                var lstTotal = allActivity.GroupBy(s => s.DesciplineId).OrderBy(s => s.Key)
                   .Select(s => s.Count()).ToList();

                aSeriesTotal["color"] = $"#{Common.ActivityStatusColor.DE1515}";

                BarChartDetails<string, object> aSeries = new BarChartDetails<string, object>();
                aSeries["name"] = ActivityStatus.Done.ToString();
                var lstDate = allActivity
                    .GroupBy(s => s.DesciplineId).OrderBy(s => s.Key)
                   .Select(s => s.Where(o=>o.Status==ActivityStatus.Done).Count()).ToList();

                aSeries["color"] = $"#{Common.ActivityStatusColor.A3db08}";

                aSeriesTotal["data"] = lstTotal;
                viewModel.Values.Add(aSeriesTotal);
                aSeries["data"] = lstDate;
                viewModel.Values.Add(aSeries);
            }
            else
            {
                var gstatus = allActivity.GroupBy(s => s.Status);

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

                    var gByStatus = gbyd.OrderBy(s => s.DesciplineId).GroupBy(s => s.DesciplineId);

                    foreach (var item in gByStatus)
                    {
                        lstDate.Add(item.Count());
                    }
                    aSeries["data"] = lstDate;
                    viewModel.Values.Add(aSeries);
                }
            }
            return viewModel;
        }

        public async Task<BarChartDto> getActivityConditionByDescipline()
        {
            var desciplens = await _context.Desciplines.OrderBy(s => s.Id).Select(s => s.Name).ToListAsync();

            var allActivity = await _context.Activites.Where(s =>s.Condition!=Common.ActivityCondition.Normal)
                .ToListAsync();

            var gstatus = allActivity.GroupBy(s => s.Condition);

            var viewModel = new BarChartDto();
            viewModel.Desciplines = desciplens;

            foreach (var gbyd in gstatus)
            {
                BarChartDetails<string, object> aSeries = new BarChartDetails<string, object>();
                aSeries["name"] = gbyd.Key.ToString();
                var lstDate = new List<int>();

                var gByStatus = gbyd.OrderBy(s => s.DesciplineId).GroupBy(s => s.DesciplineId);

                foreach (var item in gByStatus)
                {
                    lstDate.Add(item.Count());
                }
                aSeries["data"] = lstDate;
                viewModel.Values.Add(aSeries);
            }
            return viewModel;
        }

        public async Task<PieChartsListDto> getActivityCounterByDescipline()
        {
            var desciplens = await _context.Desciplines.OrderBy(s => s.Id).ToDictionaryAsync(x => x.Id, s => s.Name);

            var allActivity = await _context.Activites.ToListAsync();

            var gGroup = allActivity.GroupBy(s => s.DesciplineId);

            var viewModel = new PieChartsListDto();

        
            foreach (var gbyd in gGroup)
            {
                var desc = desciplens[gbyd.Key];
                var item = new PieChartDto
                {
                    Name = desc,
                    Y = gbyd.Count()
                };

                viewModel.Values.Add(item);
            }

            if(viewModel.Values.Any())
            {
                var maxItem = viewModel.Values.OrderByDescending(i => i.Y).First();
                maxItem.Selected = true;
                maxItem.Sliced = true;
            }

            return viewModel;
        }

        public async Task<BarChartDto> getActivityTaskDoneByDescipline()
        {
            var viewModel = new BarChartDto();

            var desciplens = await _context.Desciplines.OrderBy(s => s.Id).ToDictionaryAsync(x => x.Id, s => s.Name);

            var allActivity = await _context.Activites.Where(s =>s.Status == Common.ActivityStatus.Done &&
            s.UpdatedDate>=DateTime.Now.AddDays(-30))
            .GroupBy(s => new {s.DesciplineId,s.UpdatedDate.Date }).ToListAsync();

            List<List<object>> lstItms = null;
            BarChartDetails<string, object> aSeries = null;

            allActivity.ForEach(item =>
            {
                var desc = desciplens[item.Key.DesciplineId];

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
