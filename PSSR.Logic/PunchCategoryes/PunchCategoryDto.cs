using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Logic.PunchCategoryes
{
    public class PunchCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid ProjectId { get; set; }
    }
}
