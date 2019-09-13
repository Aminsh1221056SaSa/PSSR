using Microsoft.EntityFrameworkCore;
using PSSR.DataLayer.EfCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSSR.ServiceLayer.MDRStatuses.QueryObjects;
using PSSR.DataLayer.QueryObjects;

namespace PSSR.ServiceLayer.MDRStatuses.Concrete
{
    public class ListMDRStatusService
    {
        private readonly EfCoreContext _context;
        public ListMDRStatusService(EfCoreContext context)
        {
            _context = context;
        }

        public async Task<MDRStatusListDto> GetMdrStatus(int id)
        {
            return await _context.MDRStatus.Where(s => s.Id == id)
                .MapMDRStatusToDto().SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MDRStatusListDto>> GetAllMdrStatus()
        {
            return await _context.MDRStatus.AsNoTracking().MapMDRStatusToDto().ToListAsync();
        }

        public Task<IQueryable<MDRStatusListDto>> SortFilterPage
           (MDRStatusSortFilterPageOptions options,Guid projectId)
        {
            return Task.Run(() =>
            {
                var mdrDocsQuery = _context.MDRStatus.Where(s=>s.ProjectId==projectId).AsNoTracking().MapMDRStatusToDto();

                options.SetupRestOfDto(mdrDocsQuery);

                return mdrDocsQuery.Page(options.PageNum - 1,
                                       options.PageSize);
            });
        }
    }
}
