using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers.Security.Authorization
{
    internal class UserRoleAuthorizationHandler : AuthorizationHandler<UserRoleRequirement>
    {
        private readonly ILogger<UserRoleAuthorizationHandler> _logger;

        public UserRoleAuthorizationHandler(ILogger<UserRoleAuthorizationHandler> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRoleRequirement requirement)
        {
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
