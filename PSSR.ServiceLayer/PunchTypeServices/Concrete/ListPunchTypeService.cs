using Microsoft.EntityFrameworkCore;
using PSSR.DataLayer.EfClasses;
using PSSR.DataLayer.EfClasses.Projects.Activities;
using PSSR.DataLayer.EfCode;
using PSSR.DataLayer.QueryObjects;
using PSSR.ServiceLayer.PunchTypeServices.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.PunchTypeServices.Concrete
{
    public class ListPunchTypeService
    {
        private readonly EfCoreContext _context;

        public ListPunchTypeService(EfCoreContext context)
        {
            _context = context;
        }

        public async Task<PunchTypeListDto> GetPunchType(int id)
        {
            var item = await _context.FindAsync<PunchType>(id);
            return new PunchTypeListDto
            {
               Id=item.Id,
               Name=item.Name,
            };
        }

        public async Task<IEnumerable<PunchType>> GetProjectPunches(Guid projectId)
        {
            return await _context.PunchTypes.Where(s => s.ProjectId == projectId)
                .Include(s=>s.WorkPackages).ToListAsync();
        }

        public async Task<IEnumerable<PunchTypeListDto>> GetPunchTypes(Guid projectId)
        {
            return await Task.Run(()=>
            {
               return  _context.PunchTypes.Where(s => s.ProjectId == projectId).Include(s => s.WorkPackages)
                .ThenInclude(s => s.WorkPackage).ToList()
                .Select(item => new PunchTypeListDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    WorkPackagePr = item.WorkPackages.Select(o => $"{o.WorkPackage.Name}  :  {o.Precentage} ").Aggregate((a, b) => a + " --" + b)
                });
                });
        }

        public IQueryable<PunchTypeListDto> SortFilterPage
           (PunchTypeSortFilterPageOptions options, Guid projectId)
        {
            var punchTypeQuery = _context.PunchTypes
                .AsNoTracking()
                 .Where(item =>item.ProjectId==projectId && item.Name.Contains(options.QueryFilter) || string.IsNullOrWhiteSpace(options.QueryFilter))
                .MapPanchTypeToDto()
                .OrderPunchTypeBy(options.OrderByOptions)
                .FilterPunchTypeBy(options.FilterBy,
                               options.FilterValue);

            options.SetupRestOfDto(punchTypeQuery);

            return punchTypeQuery.Page(options.PageNum - 1,
                                   options.PageSize);
        }

        public IQueryable<PunchTypeListDto> GetAllDesciplines()
        {
            return _context.PunchTypes.AsNoTracking().MapPanchTypeToDto();
        }
    }
}
