using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSSR.Security.Helpers;
using PSSR.Security.Models;
using PSSR.UserSecurity.Configuration.IdentityContextModels;
using static IdentityServer4.IdentityServerConstants;

namespace PSSR.Security.Configuration.IdentityServer.Client
{
    namespace Refinery.Archives.Security.Configuration.IdentityServer.Client
    {
        [Authorize(Policy = "dataEventRecordsAdmin")]
        public class ClientController : Controller
        {
            private readonly ConfigurationDbContext _configContext;
            private readonly AppIdentityDbContext _context;
            public ClientController(ConfigurationDbContext configContext, AppIdentityDbContext context)
            {
                this._configContext = configContext;
                _context = context;
            }

            public IActionResult ClientList()
            {
                var viewModel = new List<IdentityServer4.Models.Client>();
                viewModel = _configContext.Clients.Select(s => s.ToModel()).ToList();

                return View(viewModel);
            }

            public IActionResult SettingClient(string clientName)
            {
                var rcleint = _configContext.Clients.Where(c => c.ClientName == clientName).FirstOrDefault();
                var viewModel = _context.NavigationMenus.Where(s => s.ClientName == rcleint.ClientName
               ).Include(s => s.Roles).ThenInclude(s => s.Role).AsEnumerable().Where(s => s.Parent == null);
                ViewBag.ClientName = clientName;
                return View(viewModel);
            }

            public IActionResult EditClient(string clientId)
            {
                var rcleint = _configContext.Clients.Where(c => c.ClientId == clientId)
                    .Include(s => s.RedirectUris).Include(s => s.PostLogoutRedirectUris).FirstOrDefault();
                var client = rcleint.ToModel();
                return View(client);
            }

            public IActionResult CreateClient()
            {
                var model = new ClientModel
                {
                    AllowOfflineAccess = true,
                    RequireConsent = false,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                };

                PropertyInfo[] grantInfo;
                grantInfo = typeof(GrantTypes).GetProperties(BindingFlags.Public |
                                                              BindingFlags.Static);
                model.AllowedGrantTypes = grantInfo.Select(s => s.Name).ToList();

                FieldInfo[] scopesInfo;
                scopesInfo = typeof(StandardScopes).GetFields(BindingFlags.Public | BindingFlags.Static |
                   BindingFlags.FlattenHierarchy)
                 .Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToArray();

                model.AllowedScopes = scopesInfo.Select(s => s.GetValue(s).ToString()).ToList();

                return View(model);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult CreateClient(ClientModel model)
            {
                if (model.SelectedScopes == null)
                {
                    ModelState.AddModelError("Client", "Please select or insert at least a scope.");
                }

                if (model.AllowedGrantType == null)
                {
                    ModelState.AddModelError("Client", "Please select a grnt type.");
                }

                if (model.RedirectUris == null)
                {
                    ModelState.AddModelError("Client", "Please insert at least a RedirectUris.");
                }

                if (model.PostLogoutRedirectUris == null)
                {
                    ModelState.AddModelError("Client", "Please insert at least a PostLogoutRedirectUris.");
                }
                var grantType = HelperExtension.GetPropValue<GrantTypes>(model.AllowedGrantType) as ICollection<string>;
                if (grantType == null)
                {
                    ModelState.AddModelError("Client", "grant Type not valid.");
                }

                if (!ModelState.IsValid)
                {
                    PropertyInfo[] grantInfo;
                    grantInfo = typeof(GrantTypes).GetProperties(BindingFlags.Public |
                                                                  BindingFlags.Static);
                    model.AllowedGrantTypes = grantInfo.Select(s => s.Name).ToList();

                    FieldInfo[] scopesInfo;
                    scopesInfo = typeof(StandardScopes).GetFields(BindingFlags.Public | BindingFlags.Static |
                       BindingFlags.FlattenHierarchy)
                     .Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToArray();

                    model.AllowedScopes = scopesInfo.Select(s => s.GetValue(s).ToString()).ToList();

                    return View(model);
                }

                var client = new IdentityServer4.Models.Client
                {
                    ClientId = Guid.NewGuid().ToString(),
                    ClientName = model.ClientName,
                    AllowedGrantTypes = grantType,
                    Description = model.Description,
                    RequireConsent = model.RequireConsent,

                    ClientSecrets =
                    {
                        new Secret(model.SecretCode.Sha256())
                    },

                    RedirectUris = model.RedirectUris,
                    PostLogoutRedirectUris = model.PostLogoutRedirectUris,

                    AllowedScopes = model.SelectedScopes,

                    AllowOfflineAccess = model.AllowOfflineAccess,
                    AlwaysSendClientClaims = model.AlwaysSendClientClaims,
                    AlwaysIncludeUserClaimsInIdToken = model.AlwaysIncludeUserClaimsInIdToken,
                };
                _configContext.Clients.Add(client.ToEntity());
                _configContext.SaveChanges();

                return RedirectToAction("ClientList");
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult EditClient(IdentityServer4.Models.Client model)
            {
                var vModel = model.ToEntity();

                var rClient = _configContext.Clients.Where(s => s.ClientId == model.ClientId).SingleOrDefault();
                rClient.ClientId = model.ClientId;
                rClient.ClientName = model.ClientName;
                rClient.RedirectUris = vModel.RedirectUris;
                rClient.PostLogoutRedirectUris = vModel.PostLogoutRedirectUris;
                rClient.Description = model.Description;
                rClient.AllowOfflineAccess = model.AllowOfflineAccess;
                rClient.AlwaysSendClientClaims = model.AlwaysSendClientClaims;
                rClient.AlwaysIncludeUserClaimsInIdToken = model.AlwaysIncludeUserClaimsInIdToken;
                rClient.RequireConsent = model.RequireConsent;

                _configContext.Update(rClient);
                _configContext.SaveChanges();
                return RedirectToAction("ClientList");
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult DisableClient(string clientId)
            {
                var rClient = _configContext.Clients.Where(s => s.ClientId == clientId).FirstOrDefault();
                if (rClient != null)
                {
                    rClient.Enabled = false;
                    _configContext.Update(rClient);
                    _configContext.SaveChanges();
                }
                return RedirectToAction("ClientList");
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult EnableClient(string clientId)
            {
                var rClient = _configContext.Clients.Where(s => s.ClientId == clientId).FirstOrDefault();
                if (rClient != null)
                {
                    rClient.Enabled = true;
                    _configContext.Update(rClient);
                    _configContext.SaveChanges();
                }
                return RedirectToAction("ClientList");
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult DeleteClient(string clientId)
            {
                var rClient = _configContext.Clients.Where(s => s.ClientId == clientId).FirstOrDefault();
                if (rClient != null)
                {
                    _configContext.Remove(rClient);
                    _configContext.SaveChanges();
                }
                return RedirectToAction("ClientList");
            }
        }
    }
}