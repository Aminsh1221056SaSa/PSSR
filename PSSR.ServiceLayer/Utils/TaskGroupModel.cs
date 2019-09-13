using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.ServiceLayer.Utils
{
    public class TaskGroupModel
    {
        public bool GroupByWorkPackage { get; set; }
        public bool GroupByLocation { get; set; }
        public bool GroupByDescipline { get; set; }
        public bool GroupBySystem { get; set; }
        public bool GroupBySubSystem { get; set; }
    }
}
