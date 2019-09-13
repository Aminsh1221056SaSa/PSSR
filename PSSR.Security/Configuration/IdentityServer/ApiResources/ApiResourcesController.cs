using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PSSR.Security.Configuration.IdentityServer.ApiResources
{
    [Authorize(Policy = "dataEventRecordsAdmin")]
    public class ApiResourcesController : Controller
    {
        private readonly ConfigurationDbContext _configContext;
        public ApiResourcesController(ConfigurationDbContext configContext)
        {
            this._configContext = configContext;
        }

        public IActionResult ApiResources()
        {
            var viewModel = new List<IdentityServer4.Models.ApiResource>();
            viewModel = _configContext.ApiResources.Select(s => s.ToModel()).ToList();

            return View(viewModel);
        }

        public IActionResult CreateApi()
        {
            return View(new IdentityServer4.Models.ApiResource());
        }

        public IActionResult EditApi(string apiName)
        {
            var rcleint = _configContext.ApiResources.Where(c => c.Name == apiName).FirstOrDefault();
            var client = rcleint.ToModel();
            return View(client);
        }

        [HttpPost]
        public IActionResult CreateApi(IdentityServer4.Models.ApiResource model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var entity = model.ToEntity();
            entity.Scopes.Add(new IdentityServer4.EntityFramework.Entities.ApiScope
            {
                Description=entity.Description,
                Name=entity.Name,
                DisplayName=entity.DisplayName,
                ShowInDiscoveryDocument=true,
                Required=false,
                Emphasize=false
            });

            var resources=_configContext.ApiResources.Add(entity);
            _configContext.SaveChanges();
            return RedirectToAction("ApiResources");
        }

        [HttpPost]
        public IActionResult EditApi(IdentityServer4.Models.ApiResource model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var rapi = _configContext.ApiResources.Where(c => c.Name == model.Name).FirstOrDefault();
            var api = model.ToEntity();
            rapi.DisplayName = api.DisplayName;
            rapi.Description = api.Description;
            rapi.UserClaims = api.UserClaims;

            _configContext.Update(rapi);
            _configContext.SaveChanges();
            return RedirectToAction("ApiResources");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DisableApi(string apiName)
        {
            var rapi = _configContext.ApiResources.Where(s => s.Name == apiName).FirstOrDefault();
            if (rapi != null)
            {
                rapi.Enabled = false;
                _configContext.Update(rapi);
                _configContext.SaveChanges();
            }
            return RedirectToAction("ApiResources");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EnableApi(string apiName)
        {
            var rapi = _configContext.ApiResources.Where(s => s.Name == apiName).FirstOrDefault();
            if (rapi != null)
            {
                rapi.Enabled = true;
                _configContext.Update(rapi);
                _configContext.SaveChanges();
            }
            return RedirectToAction("ApiResources");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteApi(string apiName)
        {
            var rapi = _configContext.ApiResources.Where(s => s.Name == apiName).FirstOrDefault();
            if (rapi != null)
            {
                _configContext.Remove(rapi);
                _configContext.SaveChanges();
            }
            return RedirectToAction("ApiResources");
        }
    }
}