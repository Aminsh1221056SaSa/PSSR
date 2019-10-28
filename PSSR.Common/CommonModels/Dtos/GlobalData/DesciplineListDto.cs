using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.Common.DesciplineServices
{
    public class DesciplineListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SubSystemCount { get; set; }
    }
}
