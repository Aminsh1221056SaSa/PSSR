using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Quickstart.UI
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("Oil_Api", "Oil App Api")
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "764345E4-7FDB-416D-99E1-900EBCB21FE2",
                    ClientName = "APSE PSSR WEB APP",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret("APSE_PSSR_APP_SECRET".Sha256())
                    },

                    RedirectUris = {"https://localhost:44385/signin-oidc"},
                    PostLogoutRedirectUris = {"https://localhost:44385/signout-callback-oidc"},

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                         IdentityServerConstants.StandardScopes.OfflineAccess,
                        "Oil_Api"
                    },

                    AllowOfflineAccess = true,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    //BackChannelLogoutSessionRequired=true,
                    //BackChannelLogoutUri="https://localhost:44385/Home/LogOutBackChannel"
                }
               
            };
        }
    }
}
