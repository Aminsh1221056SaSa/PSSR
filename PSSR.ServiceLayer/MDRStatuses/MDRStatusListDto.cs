using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.MDRStatuses
{
    public class MDRStatusListDto
    {
        public int Id { get;  set; }
        public string Name { get;  set; }
        public float Wf { get;  set; }
        public Guid ProjectId { get; set; }
        public string Description { get; set; }
    }
}
