using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Common.CommonModels
{
    public class ValueUnitModel : IEquatable<ValueUnitModel>, IFormattable
    {
        public ValueUnitModel()
        {
            this.Childeren = new List<ValueUnitModel>();
        }
        public int Id { get;  set; }
        public string Name { get;  set; }
        public UnitMathType MathType { get; set; }
        public Int32 MathNum { get;  set; }

        //-----------------------------------------
        //Relationships
        public Int32? ParentId { get; set; }

        public ICollection<ValueUnitModel> Childeren { get; private set; }

        public bool Equals(ValueUnitModel other)
        {
            if (other == null)
                return base.Equals(other);
            return this.Id == other.Id && this.Name == other.Name;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return base.Equals(obj);
            return Equals(obj as ValueUnitModel);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.Name, this.Id);
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null)
            {
                return ToString();
            }
            string formatUpper = format.ToUpper();
            switch (formatUpper)
            {
                case "URM":
                    return setMathFormat();
                default:
                    return ToString();
            }
        }

        private string setMathFormat()
        {
            switch (MathType)
            {
                case UnitMathType.None:
                    return "Not Relation";
                case UnitMathType.Divide:
                    return string.Format("Relation to Parent Value Unit 1 / {0}", MathNum);
                case UnitMathType.Multiple:
                    return string.Format("Relation to Parent Value Unit 1 * {0}", MathNum);
                default:
                    return "Not Specific";
            }
        }
    }
}
