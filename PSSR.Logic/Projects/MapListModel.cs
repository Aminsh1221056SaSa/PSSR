using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.Logic.Projects
{
    public class MapListModel
    {
        public Dictionary<int,int> Item { get; set; }
        public Guid ProjectId { get; set; }
    }
}
