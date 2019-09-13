using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Logic.Desciplines
{
    public class DesciplineDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid[] ProjectIds { get; set; }
    }
}
