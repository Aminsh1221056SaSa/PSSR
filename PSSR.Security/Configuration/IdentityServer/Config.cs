
using IdentityServer4.Models;
using System.Collections.Generic;

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
                    ClientId = "775FA1AC-3EFC-4F34-827C-6F06BA30511D",
                    ClientName = "APSE PSSR WEB APP",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret("APSE_PSSR_APP_SECRET".Sha256())
                    },

                    RedirectUris = {"https://localhost:44349/signin-oidc"},
                    PostLogoutRedirectUris = {"https://localhost:44349/signout-callback-oidc"},

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
                },
                new Client
{
    ClientId = "js",
    ClientName = "JavaScript Client",
    AllowedGrantTypes = GrantTypes.Code,
    RequirePkce = true,
    RequireClientSecret = false,

    RedirectUris =           { "https://localhost:44349/callback.html" },
    PostLogoutRedirectUris = { "https://localhost:44349/index.html" },
    AllowedCorsOrigins =     { "https://localhost:44349" },

    AllowedScopes =
    {
        IdentityServerConstants.StandardScopes.OpenId,
        IdentityServerConstants.StandardScopes.Profile,
        "Oil_Api"
    }
}

            };
        }
    }
}
