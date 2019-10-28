using Microsoft.EntityFrameworkCore;
using PSSR.Common.WorkPackageSteps;
using PSSR.DataLayer.EfCode;
using PSSR.DataLayer.QueryObjects;
using PSSR.ServiceLayer.WorkPackageSteps.QueryObjects;
using System.Collections.Generic;
using System.Linq;
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
    }
}
