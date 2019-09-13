using PSSR.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Logic.ValueUnits
{
    public class ValueUnitDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UnitMathType MathType { get; set; }
        public Int32 MathNum { get; set; }
        public Int32? ParentId { get; set; }
    }
}
