using Microsoft.AspNetCore.Mvc;
using PSSR.UI.Helpers.Navigation;
using System.Linq;

namespace PSSR.UI.ViewComponents
{
    public class RightNavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(NavigationMenu menu)
        {
            menu.MenuItems = menu.MenuItems.OrderBy(p => p.Sequence).ToList();
            return View(menu);
        }
    }
}
