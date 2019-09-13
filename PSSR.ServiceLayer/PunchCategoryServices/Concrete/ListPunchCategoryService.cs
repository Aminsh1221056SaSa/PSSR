using Microsoft.EntityFrameworkCore;
using PSSR.DataLayer.EfClasses;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DataLayer.EfCode;
using PSSR.DataLayer.QueryObjects;
using PSSR.ServiceLayer.PunchCategoryServices.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.PunchCategoryServices.Concrete
{
    public class ListPunchCategoryService
    {
        private readonly EfCoreContext _context;

        public ListPunchCategoryService(EfCoreContext context)
        {
            _context = context;
        }

        public async Task<PunchCategoryListDto> GetPunchCategory(int id)
        {
            var item = await _context.FindAsync<PunchCategory>(id);
            return new PunchCategoryListDto
            {
               Id=item.Id,
               Name=item.Name,
            };
        }

        public async Task<IEnumerable<PunchCategoryListDto>> GetProjectPuncheCategories(Guid projectId)
        {
            return await Task.Run(()=>
            {
               return  _context.PunchCategories.Where(s => s.ProjectId == projectId).ToList()
                .Select(item => new PunchCategoryListDto
                {
                    Id = item.Id,
                    Name = item.Name,
                });
                });
        }

        public IQueryable<PunchCategoryListDto> SortFilterPage
           (PunchCategorySortFilterPageOptions options,Guid projectId)
        {
            var punchTypeQuery = _context.PunchCategories
                .AsNoTracking()
                 .Where(item =>item.ProjectId==projectId && item.Name.Contains(options.QueryFilter) || string.IsNullOrWhiteSpace(options.QueryFilter))
                .MapPanchCategoryToDto()
                .OrderPunchCategoryBy(options.OrderByOptions)
                .FilterPunchCategoryBy(options.FilterBy,
                               options.FilterValue);

            options.SetupRestOfDto(punchTypeQuery);

            return punchTypeQuery.Page(options.PageNum - 1,
                                   options.PageSize);
        }

    }
}
