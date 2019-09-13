using PSSR.UserSecurity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSSR.UI.Helpers.Navigation
{
    public class NavigationMenu
    {
        public NavigationMenu()
        {
            this.MenuItems = new List<NavigationMenuItem>();
        }
        public List<NavigationMenuItem> MenuItems { get; set; }
    }

    public class NavigationMenuItem
    {
        public string DisplayName { get; set; }
        public string MaterialIcon { get; set; }
        public string Link { get; set; }
        public bool IsNested { get; set; }
        public int Sequence { get; set; }
        public MenuType Type { get; set; }
        public int? ParentId { get; set; }
        public List<string> SelectedRoles { get; set; }
        public List<NavigationMenuItem> Childeren { get; set; }
    }
}
