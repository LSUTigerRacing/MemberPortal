using Microsoft.AspNetCore.Authorization;
using TRFSAE.MemberPortal.API.Enums;

namespace TRFSAE.MemberPortal.API.Authorization;

/// <summary>
/// Requires the caller's role to be at least <see cref="MinimumRole"/> on
/// the numeric ladder defined in <see cref="RoleLevels"/> ("X and above").
/// One requirement type, parameterized per policy — see Program.cs for the
/// registered "MemberAA" / "SubsystemLeadAA" / "AdminAA" policies.
/// </summary>
public class MinimumRoleRequirement : IAuthorizationRequirement
{
    public Role MinimumRole { get; }

    public MinimumRoleRequirement(Role minimumRole)
    {
        MinimumRole = minimumRole;
    }
}
