using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.Security.Models
{
    public class ClientModel
    {
        public string ClientId { get; set; }
        [Required(ErrorMessage ="Client Name is required.")]
        public string ClientName { get; set; }
        public string Description { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        public bool RequireConsent { get; set; }
        [Required(ErrorMessage = "SecretCode is required.")]
        public string SecretCode { get; set; }
        public List<string> RedirectUris { get; set; }
        public List<string> PostLogoutRedirectUris { get; set; }
        public List<string> AllowedScopes { get; set; }
        public List<string> AllowedGrantTypes { get; set; }
        [Required(ErrorMessage = "Allowed GrantType is required.")]
        public string AllowedGrantType { get; set; }
        [Required(ErrorMessage = "Allowed Scope is required.")]
        public List<string> SelectedScopes { get; set; }
    }
}
