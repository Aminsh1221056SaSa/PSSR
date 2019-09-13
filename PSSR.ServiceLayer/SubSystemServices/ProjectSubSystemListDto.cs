using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.SubSystemServices
{
    public class ProjectSubSystemListDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public int PriorityNo { get; set; }
        public int? SubPriorityNo { get; set; }
        public string Description { get; set; }
        public int ProjectSystemId { get; set; }
        public string SystemCode { get; set; }
    }
}
