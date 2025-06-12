using Microsoft.AspNetCore.Authorization;

namespace Carting.Security
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        private const string PermissionsClaim = "permissions";

        protected override Task HandleRequirementAsync(
                                                        AuthorizationHandlerContext context, 
                                                        HasScopeRequirement requirement)
        {
            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == PermissionsClaim && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            // Split the scopes string into an array
            var scopes = context.User.Claims.Where(c => c.Type == PermissionsClaim && c.Issuer == requirement.Issuer).Select(c => c.Value);

            // Succeed if the scope array contains the required scope
            if (scopes.Any(s => s == requirement.Scope))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
