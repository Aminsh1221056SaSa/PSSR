using Microsoft.EntityFrameworkCore;
using PSSR.Common.FormDictionaryServices;
using PSSR.DataLayer.EfCode;
using PSSR.DataLayer.QueryObjects;
using PSSR.ServiceLayer.FormDictionaryServices.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.FormDictionaryServices.Concrete
{
    public class ListFormDictionaryService
    {
        private readonly EfCoreContext _context;

        public ListFormDictionaryService(EfCoreContext context)
        {
            _context = context;
        }

        public async Task<bool> HasDuplicatedCode(string code)
        {
            return await _context.FormDictionaries.AnyAsync(s => s.Code == code);
        }

        public async Task<FormDictionaryListDto> GetformDictionary(long id)
        {
            return await _context.FormDictionaries.Where(f=>f.Id==id)
                .Include(s=>s.DesciplineLink).Select(item=> new FormDictionaryListDto
            {
                Code=item.Code,
                WorkPackageId=item.WorkPackageId,
                Description=item.Description,
                Id=item.Id,
                ActivityName=item.ActivityName,
                FileName=item.FileName,
                Type=item.Type,
                DesciplinesIds = item.DesciplineLink.Select(s => s.Descipline.Id).ToList(),
                Priority=item.Priority,
                Mh=item.ManHours,
            }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<FormDictionaryListDto>> GetFormsByDescipline(int desCiplineId)
        {
            return await _context.FormDictionaries
                .Where(s => s.DesciplineLink.Any(f=>f.DesciplineId==desCiplineId)).MapFormDicToDto().ToListAsync();

        }

        public async Task<List<FormDictionarySummaryDto>> GetformDictionaryies()
        {
            var items =await _context.FormDictionaries.Select(s=>new FormDictionarySummaryDto
            {
                Id=s.Id,
                Description=s.Description,
                Code=s.Code,
                Type=s.Type,
                WrokPackageName=s.WorkPackage.Name
            }).ToListAsync();
            return items;
        }

        public IQueryable<FormDictionaryListDto> SortFilterPage
           (FormDictionarySortFilterPageOptions options)
        {
            var desciplineQuery = _context.FormDictionaries
                .AsNoTracking()
                 .Where(item => item.Code.StartsWith(options.QueryFilter) || item.Description.Contains(options.QueryFilter) 
                 || string.IsNullOrWhiteSpace(options.QueryFilter)).MapFormDicToDto()
                .OrderFormDictionaryBy(options.OrderByOptions)
                .FilterFormDictionaryBy(options.FilterBy,
                               options.FilterValue);

            options.SetupRestOfDto(desciplineQuery);

            return desciplineQuery.Page(options.PageNum - 1,
                                   options.PageSize);
        }
    }
}
