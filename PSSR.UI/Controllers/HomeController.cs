using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PSSR.UI.Helpers.Security;
using PSSR.UI.Models;

namespace PSSR.UI.Controllers
{
    public class HomeController : BaseTraceController
    {
        //private readonly LogoutUserManager _logoutUserManager;
        public HomeController()
        {
           // _logoutUserManager = logoutUser;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            return new SignOutResult(new[] {"Cookies","oidc"});
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> LogOutBackChannel(string logout_token)
        //{
        //    var user = await validateLogoutToken(logout_token);
        //    var sub = user.FindFirst("sub")?.Value;
        //    var sid = user.FindFirst("sid")?.Value;
        //    _logoutUserManager.Add(sub, sid);
        //    return Ok();
        //}

        //private async Task<ClaimsPrincipal> validateLogoutToken(string logoutToken)
        //{
        //    var claims = await JWTValidationHelper.ValidateJwt(logoutToken);
        //    if (claims.FindFirst("sub") == null && claims.FindFirst("sid") == null) throw new Exception("invaliad logout token");

        //    var nonce = claims.FindFirstValue("nonce");
        //    if(!string.IsNullOrWhiteSpace(nonce)) throw new Exception("invaliad logout token");

        //    var eventsJson = claims.FindFirst("events")?.Value;
        //    if (string.IsNullOrWhiteSpace(eventsJson)) throw new Exception("invaliad logout token");

        //    //var events = JObject.Parse(eventsJson);
        //    //var logoutevent = events.TryGetValue("");
        //    return claims;
        //}
    }
}
