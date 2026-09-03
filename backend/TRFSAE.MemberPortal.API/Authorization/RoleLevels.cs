using TRFSAE.MemberPortal.API.Enums;

namespace TRFSAE.MemberPortal.API.Authorization;

/// <summary>
/// Numeric hierarchy backing "and above" (AA) role checks throughout the
/// app. Higher number = more access. This is the single source of truth
/// for level comparisons — both policy handlers and <see cref="Interfaces.IPermissionService"/>
/// read from here.
/// </summary>
public static class RoleLevels
{
    private static readonly Dictionary<Role, int> Levels = new()
    {
        [Role.SuperAdmin] = 99,
        [Role.Admin] = 98,
        [Role.SystemLead] = 97,
        [Role.SubsystemLead] = 95,
        [Role.Member] = 5,
        [Role.Unverified] = 0
    };

    /// <summary>
    /// Any role not listed in the ladder defaults to Unverified's level (0)
    /// rather than throwing, so a future Role member added without a ladder
    /// entry fails closed instead of crashing requests.
    /// </summary>
    public static int Of(Role role) => Levels.GetValueOrDefault(role, 0);

    public static bool IsAtLeast(Role actual, Role minimum) => Of(actual) >= Of(minimum);
}
