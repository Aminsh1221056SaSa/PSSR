using Microsoft.EntityFrameworkCore;
using PSSR.Common;
using PSSR.DataLayer.EfClasses.Projects;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DataLayer.EfCode;
using PSSR.DataLayer.QueryObjects;
using PSSR.ServiceLayer.Utils.ChartsDto;
using PSSR.ServiceLayer.WorkPackageSteps.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.WorkPackageSteps.Concrete
{
    public class WorkPackageStepService
    {
        private readonly EfCoreContext _context;

        public WorkPackageStepService(EfCoreContext context)
        {
            _context = context;
        }

        public async Task<WorkPackageStepListDto> GetWorkPackageStep(int id)
        {
            var item = await _context.WorkPackageStep.Where(p=>p.Id==id).Select(p=>new WorkPackageStepListDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                WorkPackageName = p.WorkPackage.Name,
                WorkPackageId = p.WorkPackageId
            }).SingleOrDefaultAsync();
            return item;
        }

        public async Task<List<WorkPackageStepListDto>> GetWorkPackageStepByWorkPackage(int wid)
        {
            var items = await _context.WorkPackageStep.Where(p => p.WorkPackageId == wid).Select(p => new WorkPackageStepListDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                WorkPackageName = p.WorkPackage.Name,
                WorkPackageId = p.WorkPackageId
            }).ToListAsync();

            return items;
        }

        public async Task<List<WorkPackageStepListDto>> GetWorkPackageSteps()
        {
            var items = await _context.WorkPackageStep.Select(p => new WorkPackageStepListDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                WorkPackageId = p.WorkPackageId
            }).ToListAsync();

            return items;
        }

        public IQueryable<WorkPackageStepListDto> SortFilterPage
           (WorkPackageStepSortFilterPageOptions options)
        {
            var punchTypeQuery = _context.WorkPackageStep
                .AsNoTracking()
                 .Where(item => item.Title.Contains(options.QueryFilter) || string.IsNullOrWhiteSpace(options.QueryFilter))
                .MapWorkPackageStepToDto()
                .OrderWorkPackageStep(options.OrderByOptions)
                .FilterWorkPackageStepBy(options.FilterBy,
                               options.FilterValue);

            options.SetupRestOfDto(punchTypeQuery);

            return punchTypeQuery.Page(options.PageNum - 1,
                                   options.PageSize);
        }

        //reports
        public async Task<BarChartDto> getActivityStatusByWorkStepForWorkPackage(int workPackageId, int locationId,bool total,Guid projectId)
        {
            var workStep = await _context.WorkPackageStep.ToListAsync();
            var subsystemIds = await _context.ProjectSystems.Where(s => s.ProjectId == projectId)
                .SelectMany(s => s.ProjectSubSystems)
                .Select(s => s.Id).ToArrayAsync();

            var allActivity = new List<Activity>();

            if (locationId <= 0)
            {
                allActivity = await _context.Activites.Where(s =>subsystemIds.Contains(s.SubsytemId)  && s.WorkPackageId == workPackageId)
               .ToListAsync();
            }
            else
            {
                allActivity = await _context.Activites.Where(s => subsystemIds.Contains(s.SubsytemId)&& s.WorkPackageId == workPackageId
                && s.LocationId == locationId)
               .ToListAsync();
            }

            var availableSteps = allActivity.Select(s => s.WorkPackageStepId).ToArray();
            var viewModel = new BarChartDto();
            viewModel.Desciplines = workStep.Where(s => availableSteps.Contains(s.Id))
                .OrderBy(s => s.Id).Select(s => s.Title).ToList();

            if(total)
            {
                BarChartDetails<string, object> aSeriesTotal = new BarChartDetails<string, object>();
                aSeriesTotal["name"] = "Total";
                var lstTotal = allActivity.GroupBy(s => s.WorkPackageStepId).OrderBy(s => s.Key)
                        .Select(s => s.Count()).ToList();

                aSeriesTotal["color"] = $"#{Common.ActivityStatusColor.DE1515}";

                BarChartDetails<string, object> aSeries = new BarChartDetails<string, object>();
                aSeries["name"] = ActivityStatus.Done.ToString();
                var lstDate = allActivity
                     .GroupBy(s => s.WorkPackageStepId).OrderBy(s => s.Key)
                    .Select(s => s.Where(o => o.Status == ActivityStatus.Done).Count()).ToList();

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

                    var lstDate = new List<int>();

                    var gByStatus = gbyd.OrderBy(s => s.WorkPackageStepId).GroupBy(s => s.WorkPackageStepId);

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
    }
}
