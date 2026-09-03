using System.Security.Claims;
using TRFSAE.MemberPortal.API.Authorization;
using TRFSAE.MemberPortal.API.Enums;
using TRFSAE.MemberPortal.API.Interfaces;

namespace TRFSAE.MemberPortal.API.Services;

public class PermissionService : IPermissionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? Principal => _httpContextAccessor.HttpContext?.User;

    public Guid? CurrentUserId
    {
        get
        {
            var sub = Principal?.FindFirst("sub")?.Value;
            return Guid.TryParse(sub, out var id) ? id : null;
        }
    }

    public Role CurrentRole
    {
        get
        {
            var roleClaim = Principal?.FindFirst("role")?.Value;
            return roleClaim != null && Enum.TryParse<Role>(roleClaim, out var role) ? role : Role.Unverified;
        }
    }

    public bool IsFinance => bool.TryParse(Principal?.FindFirst("isFinance")?.Value, out var isFinance) && isFinance;

    public bool IsAtLeast(Role minimum) => RoleLevels.IsAtLeast(CurrentRole, minimum);

    public bool CanAssignRole(Role targetRole) =>
        IsAtLeast(Role.Admin) && RoleLevels.Of(targetRole) < RoleLevels.Of(CurrentRole);

    public bool CanDeleteProject(Guid projectAuthorId) =>
        IsAtLeast(Role.SubsystemLead) || (CurrentUserId is Guid id && id == projectAuthorId);

    public bool CanAccessFinance() => IsFinance || IsAtLeast(Role.Admin);

    public bool CanApproveOrder() => IsFinance || IsAtLeast(Role.SuperAdmin);
}
