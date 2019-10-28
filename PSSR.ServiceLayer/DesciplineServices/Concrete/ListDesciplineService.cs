using Microsoft.EntityFrameworkCore;
using PSSR.Common.DesciplineServices;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;
using PSSR.DataLayer.QueryObjects;
using PSSR.ServiceLayer.DesciplineServices.QueryObjects;
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

    }
}
