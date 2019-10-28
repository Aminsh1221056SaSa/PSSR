using PSSR.Common;
using System;

namespace PSSR.Common.ValueUnits
{
    public class ValueUnitListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UnitMathType MathType { get; set; }
        public Int32 MathNum { get; set; }
        public Int32? ParentId { get; set; }
    }
}
