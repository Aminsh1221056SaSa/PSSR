using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.UserSecurity.Models.IdentityContextModels
{
    public class NavigationMenuItemRole
    {
        public int NavigationMenuItemId { get; set; }
        public string RoleId { get; set; }
        public NavigationMenuType NavigationMenu { get; set; }
        public Role Role { get; set; }
    }
}
