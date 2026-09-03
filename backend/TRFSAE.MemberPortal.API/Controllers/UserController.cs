using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRFSAE.MemberPortal.API.DTOs;
using TRFSAE.MemberPortal.API.Enums;
using TRFSAE.MemberPortal.API.Interfaces;

namespace TRFSAE.MemberPortal.API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPermissionService _permissionService;

    public UserController(IUserService userService, IPermissionService permissionService)
    {
        _userService = userService;
        _permissionService = permissionService;
    }

    [HttpGet("list")]
    [Authorize(Policy = "MemberAA")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }


    [HttpGet("fetch")]
    [Authorize(Policy = "MemberAA")]
    public async Task<IActionResult> GetUserByIDAsync([FromQuery] Guid id)
    {
        var user = await _userService.GetUserAsync(id);
        return Ok(user);
    }

    [HttpPost("create")]
    [Authorize(Policy = "AdminAA")] // member invite
    public async Task<IActionResult> CreateUserAsync(CreateUserDto model)
    {
        // Same ceiling as UpdateUserByIdAsync — CreateUserDto.Role is required,
        // so an invite could otherwise hand out an unbounded role at creation time.
        if (!_permissionService.CanAssignRole(model.Role))
        {
            return Forbid();
        }

        var taskResult = await _userService.CreateUserAsync(model);
        return Ok(taskResult);
    }

    [HttpPatch("update")]
    [Authorize(Policy = "AdminAA")] // member management
    public async Task<IActionResult> UpdateUserByIdAsync([FromQuery] Guid id, UserUpdateDto model)
    {
        // UserUpdateDto carries a Role field — without this check, any AdminAA
        // caller could use this endpoint to sidestep RoleController's ceiling
        // (assigned role must be strictly below the caller's own) and hand out
        // e.g. SuperAdmin directly.
        if (model.Role.HasValue && !_permissionService.CanAssignRole(model.Role.Value))
        {
            return Forbid();
        }

        var taskResult = await _userService.UpdateUserAsync(id, model);
        return Ok(taskResult);
    }

    [HttpDelete("delete")]
    [Authorize(Policy = "AdminAA")] // member management
    public async Task<IActionResult> DeleteUserAsync([FromQuery] Guid id)
    {
        var taskResult = await _userService.DeleteUserAsync(id);
        return Ok(taskResult);
    }
}
