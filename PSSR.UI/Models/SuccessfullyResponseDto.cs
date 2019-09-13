using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Models
{
    public class SuccessfullyResponseDto
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }

    public class SuccessfullyResponseModelDto<T>
    {
        public int Key { get; set; }
        public string Value { get; set; }
        public T Model { get; set; }
    }
}
