using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TRFSAE.MemberPortal.API.DTOs;
using TRFSAE.MemberPortal.API.Enums;
using TRFSAE.MemberPortal.API.Interfaces;

namespace TRFSAE.MemberPortal.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }


    [HttpGet("fetch")]
    public async Task<IActionResult> GetUserByIDAsync([FromQuery] Guid id)
    {
        var user = await _userService.GetUserAsync(id);
        return Ok(user);
    }

    [HttpGet("current")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUserAsync()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier) ?? User.Claims.FirstOrDefault(c => c.Type == "sub");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
        {
            return Unauthorized();
        }
        var user = await _userService.GetCurrentUserAsync(userId);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUserAsync(CreateUserDto model)
    {
        var taskResult = await _userService.CreateUserAsync(model);
        return Ok(taskResult);
    }

    [HttpPatch("update")]
    public async Task<IActionResult> UpdateUserByIdAsync([FromQuery] Guid id, UserUpdateDto model)
    {
        var taskResult = await _userService.UpdateUserAsync(id, model);
        return Ok(taskResult);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteUserAsync([FromQuery] Guid id)
    {
        var taskResult = await _userService.DeleteUserAsync(id);
        return Ok(taskResult);
    }
}
