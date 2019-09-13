using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Logic.ProjectSubSystems
{
    public class ProjectSubSystemDto
    {
        public long Id { get; set; }
        public string Code { get;  set; }
        public int PriorityNo { get; set; }
        public int? SubPriorityNo { get; set; }
        public string Description { get; set; }
        public int ProjectSystemId { get; set; }
    }
}
