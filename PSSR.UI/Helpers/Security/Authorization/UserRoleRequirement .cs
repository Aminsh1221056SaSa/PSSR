using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers.Security.Authorization
{
    internal class UserRoleRequirement : IAuthorizationRequirement
    {
        public string[] UserType { get; private set; }

        public UserRoleRequirement(string[] userType)
        {
            this.UserType = userType;
        }
    }
}
