using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRFSAE.MemberPortal.API.Enums;
using TRFSAE.MemberPortal.API.Models;
using TRFSAE.MemberPortal.API.Interfaces;

namespace TRFSAE.MemberPortal.API.Controllers;

[ApiController]
[Route("api/user/role")]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly IPermissionService _permissionService;

    public RoleController(IRoleService roleService, IPermissionService permissionService)
    {
        _roleService = roleService;
        _permissionService = permissionService;
    }

    [HttpGet("fetch")]
    [Authorize(Policy = "MemberAA")]
    public async Task<IActionResult> GetUserRole([FromQuery] Guid id)
    {
        var role = await _roleService.GetUserRoleAsync(id);
        return Ok(role);
    }

    [HttpPut("update")]
    [Authorize(Policy = "AdminAA")]
    public async Task<IActionResult> AssignRoleToUser([FromQuery] Guid id, Role role)
    {
        // Ceiling rule: a caller may only hand out a role strictly below
        // their own (SuperAdmin can assign Admin/SystemLead/etc, Admin can
        // assign SystemLead/SubsystemLead/Member, but never their own role
        // or higher — prevents self-escalation and lateral promotion).
        if (!_permissionService.CanAssignRole(role))
        {
            return Forbid();
        }

        await _roleService.AssignRoleToUserAsync(id, role);
        return Ok(new { message = $"Change role to {role}" });
    }

    [HttpDelete("delete")]
    [Authorize(Policy = "AdminAA")]
    public async Task<IActionResult> RemoveUserRole([FromQuery] Guid id)
    {
        await _roleService.RemoveUserRoleAsync(id);
        return Ok(new { message = "Change role to Member" });
    }
}
