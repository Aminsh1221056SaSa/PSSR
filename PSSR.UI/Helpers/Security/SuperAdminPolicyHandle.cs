using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers.Security
{
    public class SuperAdminPolicyHandle : AuthorizationHandler<PersonPolicyRequirment>
    {
        public SuperAdminPolicyHandle()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PersonPolicyRequirment requirement)
        {
            if (!requirement.CheckUserType)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var role = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (string.IsNullOrWhiteSpace(role))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            if (requirement.UserType.Contains(role))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
