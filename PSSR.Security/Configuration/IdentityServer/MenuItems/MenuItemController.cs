using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSSR.UserSecurity.Configuration.IdentityContextModels;

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
        public IActionResult Index()
        {
            var viewModel = _context.NavigationMenus.Where(s=>s.Type==UserSecurity.Models.MenuType.PCMSWEBLeft
            ).Include(s => s.Roles).ThenInclude(s=>s.Role).ToList();
            return View(viewModel);
        }

        public IActionResult MenuInsert()
        {
            ViewBag.Roles = _context.Roles.ToList();
            ViewBag.Parents = _context.NavigationMenus.Where(s => s.Parent == null && 
            s.Type == UserSecurity.Models.MenuType.PCMSWEBLeft).ToList();

            return View();
        }

        public IActionResult EditMenu(int id)
        {
            ViewBag.Roles = _context.Roles.ToList();
            ViewBag.Parents = _context.NavigationMenus.Where(s => s.Parent == null &&
            s.Type == UserSecurity.Models.MenuType.PCMSWEBLeft).ToList();
            var viewModel = _context.NavigationMenus.Where(s => s.Id == id)
                .Include(s => s.Roles).ThenInclude(s => s.Role).Single();
            if(viewModel.Roles.Any())
            viewModel.SelectedRoles = viewModel.Roles.Select(s => s.Role.Name).ToList();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult MenuInsert(PSSR.UserSecurity.Models.NavigationMenuType Model)
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

            foreach(var m in Model.SelectedRoles)
            {
                Model.Roles.Add(new UserSecurity.Models.IdentityContextModels.NavigationMenuItemRole
                {
                    RoleId=m,
                });
            }
            Model.Type = UserSecurity.Models.MenuType.PCMSWEBLeft;
            _context.NavigationMenus.Add(Model);
            _context.SaveChanges();
            return RedirectToAction("MenuInsert");
        }

        [HttpPost]
        public IActionResult EditMenu(PSSR.UserSecurity.Models.NavigationMenuType Model)
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
            var item = _context.NavigationMenus.Where(s => s.Id == Model.Id).Include(s=>s.Roles).First();
            item.DisplayName = Model.DisplayName;
            item.Sequence = Model.Sequence;
            item.IsNested = Model.IsNested;
            item.Link = Model.Link;
            item.MaterialIcon = Model.MaterialIcon;

            var lRole = item.Roles.ToList();
            foreach(var m in lRole)
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
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteMenu(int menuId)
        {
            var menu = _context.NavigationMenus.Find(menuId);
            _context.Remove(menu);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region right menu

        public IActionResult IndexRigth()
        {
            var viewModel = _context.NavigationMenus.Where(s => s.Type == UserSecurity.Models.MenuType.PCMSWEBRight
            ).Include(s => s.Roles).ThenInclude(s => s.Role).ToList();
            return View(viewModel);
        }

        public IActionResult MenuInsertRigth()
        {
            ViewBag.Roles = _context.Roles.ToList();
            ViewBag.Parents = _context.NavigationMenus.Where(s => s.Parent == null &&
            s.Type == UserSecurity.Models.MenuType.PCMSWEBRight).ToList();

            return View();
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
        public IActionResult MenuInsertRigth(PSSR.UserSecurity.Models.NavigationMenuType Model)
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
                Model.Roles.Add(new UserSecurity.Models.IdentityContextModels.NavigationMenuItemRole
                {
                    RoleId = m,
                });
            }
            Model.Type = UserSecurity.Models.MenuType.PCMSWEBRight;
            _context.NavigationMenus.Add(Model);
            _context.SaveChanges();
            return RedirectToAction("MenuInsertRigth");
        }

        [HttpPost]
        public IActionResult EditMenuRigth(PSSR.UserSecurity.Models.NavigationMenuType Model)
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
            return RedirectToAction("IndexRigth");
        }

        [HttpPost]
        public IActionResult DeleteMenuRigth(int menuId)
        {
            var menu = _context.NavigationMenus.Find(menuId);
            _context.Remove(menu);
            _context.SaveChanges();
            return RedirectToAction("IndexRigth");
        }

        #endregion

    }
}