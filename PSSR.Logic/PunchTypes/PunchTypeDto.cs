using System;
using System.Collections.Generic;

namespace PSSR.Logic.PunchTypes
{
    public class PunchTypeDto
    {
        public int Id { get; set; }
        public string Name { get;  set; }
        public Dictionary<int, float> WorkPackagepr { get; set; }
        public Guid ProjectId { get; set; }
    }
}
