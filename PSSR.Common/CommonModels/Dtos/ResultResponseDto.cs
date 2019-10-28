using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PSSR.Common.CommonModels.Dtos
{
    public class ResultResponseDto<T,Tm>
    {
       public HttpStatusCode Key { get; set; }
        public T Value { get; set; }
        public Tm Subject { get; set; }
    }
}
