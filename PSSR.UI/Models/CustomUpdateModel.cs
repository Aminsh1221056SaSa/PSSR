using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Models
{
    public class CustomUpdateModel<T>
    {
        public long Key { get; set; }
        public T Value { get; set; }
    }
}
