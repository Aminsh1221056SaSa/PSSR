using System;

namespace PSSR.Logic.MDRStatuses
{
    public class MDRStatusDto
    {
        public int Id { get; set; }
        public string Name { get;  set; }
        public float Wf { get;  set; }
        public Guid ProjectId { get; set; }
        public string Description { get; set; }
    }
}
