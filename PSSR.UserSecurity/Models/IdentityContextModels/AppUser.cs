
using Microsoft.AspNetCore.Identity;
using PSSR.UserSecurity.Models.IdentityContextModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PSSR.UserSecurity.Models
{
    public class AppUser : IdentityUser<string>
    {
        public AppUser() { }
        public AppUser(string userName)
            : base(userName) { }

        public int PersonId { get; set; }
        [NotMapped]
        public string RoleName { get; set; }
    }

    public class Role : IdentityRole<string>
    {
        public Role()
        {
            this.NavigationItems = new List<NavigationMenuItemRole>();
        }
        public Role(string roleName) : base(roleName)
        {
            this.NavigationItems = new List<NavigationMenuItemRole>();
        }

        public ICollection<NavigationMenuItemRole> NavigationItems { get; private set; }
    }
}
