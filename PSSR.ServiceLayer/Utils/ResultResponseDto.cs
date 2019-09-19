using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PSSR.ServiceLayer.Utils
{
    public class ResultResponseDto<T,TM>
    {
        public HttpStatusCode Key { get; set; }
        public T Value { get; set; }
        public TM Subject { get; set; }
    }
}
