
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSSR.UserSecurity.Configuration.IdentityContextModels;
using PSSR.UserSecurity.Models;
using PSSR.UserSecurity.Models.IdentityContextModels;

namespace PSSR.Security.Configuration.IdentityServer.Account
{
    [Authorize(Policy = "dataEventRecordsAdmin")]
    public class MenuItemController : Controller
    {
        private readonly AppIdentityDbContext _context;
        public MenuItemController(AppIdentityDbContext context)
        {
            _context = context;
        }

        #region web left menu

        public IActionResult MenuInsert(string clientName)
        {
            ViewBag.Roles = _context.Roles.ToList();
            ViewBag.Parents = _context.NavigationMenus.Where(s => s.Parent == null &&
            s.Type == UserSecurity.Models.MenuType.PCMSWEBLeft).ToList();
            var viewModel = new NavigationMenuType { ClientName = clientName };
            return View(viewModel);
        }

        public IActionResult EditMenu(int id)
        {
            ViewBag.Roles = _context.Roles.ToList();
            ViewBag.Parents = _context.NavigationMenus.Where(s => s.Parent == null &&
            s.Type == UserSecurity.Models.MenuType.PCMSWEBLeft).ToList();
            var viewModel = _context.NavigationMenus.Where(s => s.Id == id)
                .Include(s => s.Roles).ThenInclude(s => s.Role).Single();
            if (viewModel.Roles.Any())
                viewModel.SelectedRoles = viewModel.Roles.Select(s => s.Role.Name).ToList();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult MenuInsert(NavigationMenuType Model)
        {
            if (!Model.IsNested)
            {
                if (Model.ParentId == null)
                {
                    ModelState.AddModelError("", "Please Select a parent");
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _context.Roles.ToList();
                ViewBag.Parents = _context.NavigationMenus.Where(s => s.Parent == null &&
                s.Type == UserSecurity.Models.MenuType.PCMSWEBLeft).ToList();
                return View(Model);
            }

            foreach (var m in Model.SelectedRoles)
            {
                Model.Roles.Add(new UserSecurity.Models.IdentityContextModels.NavigationMenuItemRole
                {
                    RoleId = m,
                });
            }
            Model.Type = UserSecurity.Models.MenuType.PCMSWEBLeft;
            _context.NavigationMenus.Add(Model);
            _context.SaveChanges();
            return RedirectToAction("MenuInsert", new { clientName = Model.ClientName });
        }

        [HttpPost]
        public IActionResult EditMenu(NavigationMenuType Model)
        {
            if (Model.IsNested)
            {
                if (Model.ParentId == null)
                {
                    ModelState.AddModelError("", "Please Select a parent");
                }
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _context.Roles.ToList();
                ViewBag.Parents = _context.NavigationMenus.Where(s => s.Parent == null &&
               s.Type == UserSecurity.Models.MenuType.PCMSWEBLeft).ToList();
                return View(Model);
            }
            var item = _context.NavigationMenus.Where(s => s.Id == Model.Id).Include(s => s.Roles).First();
            item.DisplayName = Model.DisplayName;
            item.Sequence = Model.Sequence;
            item.IsNested = Model.IsNested;
            item.Link = Model.Link;
            item.MaterialIcon = Model.MaterialIcon;

            var lRole = item.Roles.ToList();
            foreach (var m in lRole)
            {
                item.Roles.Remove(m);
            }
            _context.Update(item);
            _context.SaveChanges();

            foreach (var m in Model.SelectedRoles)
            {
                item.Roles.Add(new UserSecurity.Models.IdentityContextModels.NavigationMenuItemRole
                {
                    RoleId = m,
                });
            }
            _context.Update(item);
            _context.SaveChanges();
            return RedirectToAction("SettingClient", "Client", new { clientName = item.ClientName });
        }

        [HttpPost]
        public IActionResult DeleteMenu(int menuId)
        {
            var menu = _context.NavigationMenus.Find(menuId);
            _context.Remove(menu);
            _context.SaveChanges();
            return RedirectToAction("SettingClient", "Client", new { clientName = menu.ClientName });
        }
        #endregion

        #region right menu

        public IActionResult MenuInsertRigth(string clientName)
        {
            ViewBag.Roles = _context.Roles.ToList();
            ViewBag.Parents = _context.NavigationMenus.Where(s => s.Parent == null &&
            s.Type == UserSecurity.Models.MenuType.PCMSWEBRight).ToList();
            var viewModel = new NavigationMenuType { ClientName = clientName };
            return View(viewModel);
        }

        public IActionResult EditMenuRigth(int id)
        {
            ViewBag.Roles = _context.Roles.ToList();
            ViewBag.Parents = _context.NavigationMenus.Where(s => s.Parent == null &&
            s.Type == UserSecurity.Models.MenuType.PCMSWEBRight).ToList();
            var viewModel = _context.NavigationMenus.Where(s => s.Id == id)
                .Include(s => s.Roles).ThenInclude(s => s.Role).Single();
            if (viewModel.Roles.Any())
                viewModel.SelectedRoles = viewModel.Roles.Select(s => s.Role.Name).ToList();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult MenuInsertRigth(NavigationMenuType Model)
        {
            if (!Model.IsNested)
            {
                if (Model.ParentId == null)
                {
                    ModelState.AddModelError("", "Please Select a parent");
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _context.Roles.ToList();
                ViewBag.Parents = _context.NavigationMenus.Where(s => s.Parent == null &&
            s.Type == UserSecurity.Models.MenuType.PCMSWEBRight).ToList();
                return View(Model);
            }

            foreach (var m in Model.SelectedRoles)
            {
                Model.Roles.Add(new NavigationMenuItemRole
                {
                    RoleId = m,
                });
            }
            Model.Type = UserSecurity.Models.MenuType.PCMSWEBRight;
            _context.NavigationMenus.Add(Model);
            _context.SaveChanges();
            return RedirectToAction("MenuInsertRigth", new { clientName = Model.ClientName });
        }

        [HttpPost]
        public IActionResult EditMenuRigth(NavigationMenuType Model)
        {
            if (!Model.IsNested)
            {
                if (Model.ParentId == null)
                {
                    ModelState.AddModelError("", "Please Select a parent");
                }
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _context.Roles.ToList();
                ViewBag.Parents = _context.NavigationMenus.Where(s => s.Parent == null &&
               s.Type == UserSecurity.Models.MenuType.PCMSWEBRight).ToList();
                return View(Model);
            }
            var item = _context.NavigationMenus.Where(s => s.Id == Model.Id).Include(s => s.Roles).First();
            item.DisplayName = Model.DisplayName;
            item.Sequence = Model.Sequence;
            item.IsNested = Model.IsNested;
            item.Link = Model.Link;
            item.MaterialIcon = Model.MaterialIcon;

            var lRole = item.Roles.ToList();
            foreach (var m in lRole)
            {
                item.Roles.Remove(m);
            }
            _context.Update(item);
            _context.SaveChanges();

            foreach (var m in Model.SelectedRoles)
            {
                item.Roles.Add(new UserSecurity.Models.IdentityContextModels.NavigationMenuItemRole
                {
                    RoleId = m,
                });
            }
            _context.Update(item);
            _context.SaveChanges();
            return RedirectToAction("SettingClient", "Client", new { clientName = item.ClientName });
        }

        [HttpPost]
        public IActionResult DeleteMenuRigth(int menuId)
        {
            var menu = _context.NavigationMenus.Find(menuId);
            _context.Remove(menu);
            _context.SaveChanges();
            return RedirectToAction("SettingClient", "Client", new { clientName = menu.ClientName });
        }

        #endregion

    }
}