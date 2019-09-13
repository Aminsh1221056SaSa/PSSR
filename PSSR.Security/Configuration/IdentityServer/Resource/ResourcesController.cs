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
using PSSR.Security.Models;

namespace PSSR.Security.Configuration.IdentityServer.Resource
{
    [Authorize(Policy = "dataEventRecordsAdmin")]
    public class ResourcesController : Controller
    {
        private readonly ConfigurationDbContext _configContext;
        public ResourcesController(ConfigurationDbContext configContext)
        {
            this._configContext = configContext;
        }

        public IActionResult ResourcesList()
        {
            var viewModel = _configContext.IdentityResources.Select(x => x.ToModel()).ToList();
            return View(viewModel);
        }

        public IActionResult CreateIdentityResource()
        {
            return View(new ClientModel());
        }

        [HttpPost]
        public IActionResult CreateIdentityResource(ClientModel model)
        {
            foreach(var st in model.AllowedScopes)
            {
                Assembly assem = typeof(IdentityServer4.Models.IdentityResource).Assembly;
               var g= typeof(IdentityServer4.Models.IdentityResources)
                    .Assembly.GetTypes().Where(t => t.IsClass && t.Name==st).FirstOrDefault();

                if (g != null)
                {
                   var rType=  Activator.CreateInstance(g) as IdentityResource;

                    if (rType != null)
                    {
                        if(!_configContext.IdentityResources.Any(s=>s.Name==rType.Name))
                        {
                            _configContext.IdentityResources.Add(rType.ToEntity());
                        }
                    }
                }
            }
            _configContext.SaveChanges();
            return RedirectToAction("ResourcesList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DisableResource(string apiName)
        {
            var rapi = _configContext.IdentityResources.Where(s => s.Name == apiName).FirstOrDefault();
            if (rapi != null)
            {
                rapi.Enabled = false;
                _configContext.Update(rapi);
                _configContext.SaveChanges();
            }
            return RedirectToAction("ResourcesList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EnableResource(string apiName)
        {
            var rapi = _configContext.IdentityResources.Where(s => s.Name == apiName).FirstOrDefault();
            if (rapi != null)
            {
                rapi.Enabled = true;
                _configContext.Update(rapi);
                _configContext.SaveChanges();
            }
            return RedirectToAction("ResourcesList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteResource(string apiName)
        {
            var rapi = _configContext.IdentityResources.Where(s => s.Name == apiName).FirstOrDefault();
            if (rapi != null)
            {
                _configContext.Remove(rapi);
                _configContext.SaveChanges();
            }
            return RedirectToAction("ResourcesList");
        }
    }
}