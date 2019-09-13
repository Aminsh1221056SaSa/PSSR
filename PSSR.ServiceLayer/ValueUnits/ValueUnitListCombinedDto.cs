using Microsoft.AspNetCore.Mvc.Rendering;
using PSSR.ServiceLayer.DesciplineServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.ValueUnits
{
    public class ValueUnitListCombinedDto
    {
        public ValueUnitListCombinedDto(IEnumerable<ValueUnitListDto> valueUnitList)
        {
            ValueUnitList = valueUnitList;
        }

        public IEnumerable<ValueUnitListDto> ValueUnitList { get; private set; }
    }
}
