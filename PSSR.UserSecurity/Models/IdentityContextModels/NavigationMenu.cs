using PSSR.UserSecurity.Models.IdentityContextModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PSSR.UserSecurity.Models
{
    public class NavigationMenuType
    {
        public NavigationMenuType()
        {
            this.Childeren = new List<NavigationMenuType>();
            this.Roles = new List<NavigationMenuItemRole>();
        }

        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string MaterialIcon { get; set; }
        public string Link { get; set; }
        public bool IsNested { get; set; }
        public int Sequence { get; set; }
        public MenuType Type { get; set; }
        public int? ParentId { get; set; }
        public NavigationMenuType Parent { get; set; }
        public ICollection<NavigationMenuType> Childeren { get; private set; }
        public ICollection<NavigationMenuItemRole> Roles { get; private set; }

        [NotMapped]
        public List<string> SelectedRoles { get; set; }
    }

    public enum MenuType
    {
        PCMSWEBRight=1,
        PCMSWEBLeft=2
    }
}
