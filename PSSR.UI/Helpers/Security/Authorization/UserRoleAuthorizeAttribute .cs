using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers.Security.Authorization
{
    public class UserRoleAuthorizeAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "UserRole";

        public UserRoleAuthorizeAttribute(string[] userType)
        {
            this.UserType = userType;
        }

        public string[] UserType
        {
            get
            {
                return default(string[]);
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value.ToString()}";
            }
        }
    }
}
