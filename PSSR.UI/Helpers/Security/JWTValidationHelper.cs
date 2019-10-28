using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers.Security
{
    public static class JWTValidationHelper
    {
        //public static async Task<ClaimsPrincipal> ValidateJwt1(string jwt)
        //{
        //    var discoveryClient = new DiscoveryClient("https://localhost:44365");
        //    var doc = await discoveryClient.GetAsync();
        //    var keys = new List<SecurityKey>();
        //    foreach (var webkey in doc.KeySet.Keys)
        //    {
        //        var e = Base64Url.Decode(webkey.E);
        //        var n = Base64Url.Decode(webkey.N);

        //        var key = new RsaSecurityKey(new RSAParameters { Exponent = e, Modulus = n })
        //        {
        //            KeyId = webkey.Kid
        //        };
        //        keys.Add(key);
        //    }
        //    var parameters = new TokenValidationParameters
        //    {
        //        ValidIssuer = doc.Issuer,
        //        ValidAudience = "mvc",
        //        IssuerSigningKeys = keys,

        //        NameClaimType = JwtClaimTypes.Name,
        //        RoleClaimType = "role",
        //    };
        //    var handler = new JwtSecurityTokenHandler();
        //    handler.InboundClaimTypeMap.Clear();
        //    var user = handler.ValidateToken(jwt, parameters, out var _);
        //    return user;
        //}
    }
}
