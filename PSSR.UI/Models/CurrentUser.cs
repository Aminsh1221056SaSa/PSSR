using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Models
{
    public class CurrentUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string[] Roles { get; set; }
        public string Actor { get; set; }
    }
}
