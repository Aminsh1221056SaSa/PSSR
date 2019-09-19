using Microsoft.EntityFrameworkCore;
using PSSR.DataLayer.EfCode;
using PSSR.DataLayer.QueryObjects;
using PSSR.ServiceLayer.ContractorServices.QueryObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.ContractorServices.Concrete
{
    public class ListContractorService
    {
        private readonly EfCoreContext _context;

        public ListContractorService(EfCoreContext context)
        {
            _context = context;
        }

        public async Task<ContractorListDto> GetCurrentContractors(int id)
        {
            return await _context.Contractors.Where(s=>s.Id==id).Select(s => new ContractorListDto
            {
                Id=s.Id,
                Name=s.Name,
                ContractDate= s.ContractDate != null ? s.ContractDate.Value.ToString("d") : "[N/A]",
                Address =s.Address,
                PhoneNumber=s.PhoneNumber
            }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ContractorListDto>> GetAllContractors()
        {
            return await _context.Contractors.OrderByDescending(s => s.UpdatedDate).Select(s => new ContractorListDto
            {
                Id = s.Id,
                Name = s.Name,
                ContractDate = s.ContractDate != null ? s.ContractDate.Value.ToString("d") : "[N/A]",
                Address = s.Address,
                PhoneNumber = s.PhoneNumber
            }).ToListAsync();
        }

        public Task<IQueryable<ContractorListDto>> SortFilterPage(ContractorSortFilterPageOptions options)
        {
            return Task.Run(() =>
            {
                var contractorQuery = _context.Contractors
               .AsNoTracking()
               .Where(item => item.Name.Contains(options.QueryFilter) || string.IsNullOrWhiteSpace(options.QueryFilter))
               .MapContractorToDto()
               .FilterContractorBy(options.FilterBy, options.FilterValue)
               .OrderContractorBy(options.OrderByOptions);

                return contractorQuery.Page(options.PageNum - 1, options.PageSize);
            });
        }
    }
}
