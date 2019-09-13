using PSSR.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers.Security
{
    public static class ClaimsPrincipalExtensions
    {
        public static CurrentUser GetCurrentUserDetails(this ClaimsPrincipal principal)
        {
            if (!principal.Claims.Any())
                return null;

            return new CurrentUser
            {
                Name = principal.Claims.Where(c => c.Type ==ClaimTypes.Name).Select(c =>
                c.Value).SingleOrDefault(),
                Actor = principal.Claims.Where(c => c.Type == ClaimTypes.Actor).Select(c =>
                c.Value).SingleOrDefault(),
                Email = principal.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c =>
                c.Value).SingleOrDefault(),
                Roles = principal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c =>
                c.Value).ToArray()
            };
        }
    }
}
