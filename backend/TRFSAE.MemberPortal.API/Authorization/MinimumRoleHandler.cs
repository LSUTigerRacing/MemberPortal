using Microsoft.AspNetCore.Authorization;
using TRFSAE.MemberPortal.API.Enums;

namespace TRFSAE.MemberPortal.API.Authorization;

/// <summary>
/// Reads the "role" claim (populated in Program.cs's OnTokenValidated from
/// the user's DB row, not from the Supabase JWT itself) and succeeds if it
/// meets the requirement's minimum level.
/// </summary>
public class MinimumRoleHandler : AuthorizationHandler<MinimumRoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumRoleRequirement requirement)
    {
        var roleClaim = context.User.FindFirst("role")?.Value;

        if (roleClaim != null
            && Enum.TryParse<Role>(roleClaim, out var role)
            && RoleLevels.IsAtLeast(role, requirement.MinimumRole))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
