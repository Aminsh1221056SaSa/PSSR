using Microsoft.EntityFrameworkCore;
using PSSR.DataLayer.EfClasses;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.ValueUnits.Concrete
{
    public class ListValueUnitService
    {
        private readonly EfCoreContext _context;

        public ListValueUnitService(EfCoreContext context)
        {
            _context = context;
        }

        public async Task<ValueUnitListDto> GetValueUnit(int id)
        {
            var item = await _context.FindAsync<ValueUnit>(id);
            return new ValueUnitListDto
            {
                Id = item.Id,
                Name = item.Name,
               MathNum=item.MathNum,
               MathType=item.MathType,
               ParentId=item.ParentId
            };
        }

        public async Task<List<ValueUnitListDto>> GetValueUnitDtos()
        {
           return await _context.ValueUnits.Select(item => new ValueUnitListDto
            {
                Id = item.Id,
                Name = item.Name,
                MathNum = item.MathNum,
                MathType = item.MathType,
                ParentId = item.ParentId
            }).ToListAsync();
        }

        public IQueryable<ValueUnit> GetAllValueUnits()
        {
            return _context.ValueUnits.Include(s=>s.Parent).Include(s=>s.Childeren);
        }
    }
}
