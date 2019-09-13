using Microsoft.AspNetCore.Mvc;
using PSSR.UI.Helpers.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.ViewComponents
{
    public class LeftNavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(NavigationMenu menu)
        {
            menu.MenuItems = menu.MenuItems.OrderBy(p => p.Sequence).ToList();
            return View(menu);
        }
    }
}
