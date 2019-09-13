using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.ProjectServices
{
    public class ProjectMapDto
    {
        public Guid ProjectId { get;  set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long Id { get;  set; }
        public int HId { get; set; }
    }
}
