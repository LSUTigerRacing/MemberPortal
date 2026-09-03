using TRFSAE.MemberPortal.API.Enums;

namespace TRFSAE.MemberPortal.API.Interfaces;

/// <summary>
/// Resolves the current request's caller-level permissions from their
/// authentication claims. Backs the resource-scoped / cross-cutting rules
/// that don't fit into a single ASP.NET Core [Authorize(Policy = "...")]
/// gate — e.g. "SubsystemLead or the resource's own creator", Finance's
/// non-ladder access, and the role-assignment ceiling. Coarse "must be at
/// least X" gates should still use the MemberAA/SubsystemLeadAA/AdminAA
/// policies registered in Program.cs; call into this service from inside
/// the action for the rules that need more than that.
/// </summary>
public interface IPermissionService
{
    Guid? CurrentUserId { get; }
    Role CurrentRole { get; }
    bool IsFinance { get; }

    bool IsAtLeast(Role minimum);

    /// <summary>Admin AA, and only for a role strictly below the caller's own.</summary>
    bool CanAssignRole(Role targetRole);

    /// <summary>SubsystemLead AA, or the caller is the project's author.</summary>
    bool CanDeleteProject(Guid projectAuthorId);

    /// <summary>Financial dashboard "purchasing": Finance flag or Admin AA.</summary>
    bool CanAccessFinance();

    /// <summary>Order approve/reject: Finance flag (SuperAdmin bypasses everything).</summary>
    bool CanApproveOrder();
}
