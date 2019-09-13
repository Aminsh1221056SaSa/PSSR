using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers.Security
{
    public class CookieEventHandler: CookieAuthenticationEvents
    {
        public CookieEventHandler(LogoutUserManager logoutUserManager)
        {
            this.LogoutUsers = logoutUserManager;
        }

        public LogoutUserManager LogoutUsers { get; }
        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            if (context.Principal.Identity.IsAuthenticated)
            {
                var sub = context.Principal.FindFirst("sub")?.Value;
                var sid= context.Principal.FindFirst("sid")?.Value;

                if(LogoutUsers.IsLoggedOut(sub,sid))
                {
                    context.RejectPrincipal();
                    await context.HttpContext.SignOutAsync();
                }
            }
        }
    }
}
