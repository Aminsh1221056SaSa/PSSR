using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using PSSR.UserSecurity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PSSR.Security.Configuration.IdentityServer
{
    //public class IdentityWithAdditionalClaimsProfileService : IProfileService
    //{
    //    private readonly IUserClaimsPrincipalFactory<AppUser> _claimsFactory;
    //    private readonly UserManager<AppUser> _userManager;

    //    public IdentityWithAdditionalClaimsProfileService(UserManager<AppUser> userManager,
    //        IUserClaimsPrincipalFactory<AppUser> claimsFactory)
    //    {
    //        _userManager = userManager;
    //        _claimsFactory = claimsFactory;
    //    }

    //    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    //    {
    //        var sub = context.Subject.GetSubjectId();

    //        var user = await _userManager.FindByIdAsync(sub);
    //        var principal = await _claimsFactory.CreateAsync(user);

    //        var claims = principal.Claims.ToList();

    //        claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

    //        claims.Add(new Claim(JwtClaimTypes.GivenName, user.UserName));

    //        claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email));


    //        context.IssuedClaims = claims;
    //    }

    //    public async Task IsActiveAsync(IsActiveContext context)
    //    {
    //        var sub = context.Subject.GetSubjectId();
    //        var user = await _userManager.FindByIdAsync(sub);
    //        context.IsActive = user != null;
    //    }
    //}

    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsFactory;
        private readonly UserManager<AppUser> _userManager;

        public ProfileService(UserManager<AppUser> userManager, 
            IUserClaimsPrincipalFactory<AppUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //context.IssuedClaims.AddRange(context.Subject.Claims);
            var sub = context.Subject.GetSubjectId();

            var user = await _userManager.FindByIdAsync(sub);
            var claims = await _userManager.GetClaimsAsync(user);
        
            context.IssuedClaims.AddRange(claims);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(0);
        }
    }
}
