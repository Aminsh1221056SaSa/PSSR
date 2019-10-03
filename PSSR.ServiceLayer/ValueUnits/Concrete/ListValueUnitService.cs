using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PSSR.Common.CommonModels;
using PSSR.DataLayer.EfClasses.Management;
using PSSR.DataLayer.EfCode;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.ValueUnits.Concrete
{
    public class ListValueUnitService
    {
        private readonly EfCoreContext _context;
        private readonly IMapper _mapper;

        public ListValueUnitService(EfCoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ListValueUnitService(EfCoreContext context) : this(context, null) { }

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

        public async Task<IEnumerable<ValueUnitModel>> GetValueUnitTreeFormat()
        {
            var items = await _context.ValueUnits.ToListAsync();

            var pItems = items.Where(s => s.Parent == null).ToList();
            var itemsDto = _mapper.Map<List<ValueUnit>, List<ValueUnitModel>>(pItems);

            return itemsDto;
        }
    }
}
