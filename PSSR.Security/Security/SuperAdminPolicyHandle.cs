using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.Security
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

            var role = context.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

            if (string.IsNullOrWhiteSpace(role))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            if (string.Equals(role, "APSE", StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
