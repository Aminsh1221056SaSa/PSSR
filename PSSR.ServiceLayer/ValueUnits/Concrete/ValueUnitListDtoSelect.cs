using PSSR.Common.ValueUnits;
using PSSR.DataLayer.EfClasses;
using PSSR.DataLayer.EfClasses.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.ValueUnits.Concrete
{
    public static class ValueUnitListDtoSelect
    {
        public static IQueryable<ValueUnitListDto>
            MapValueUnitToDto(this IQueryable<ValueUnit> valueunits)
        {
            return valueunits.Select(p => new ValueUnitListDto
            {
                Id = p.Id,
                Name = p.Name,
                ParentId=p.ParentId,
                MathNum=p.MathNum,
                MathType=p.MathType
            });
        }
    }
}
