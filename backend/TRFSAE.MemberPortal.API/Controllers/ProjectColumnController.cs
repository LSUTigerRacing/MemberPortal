using Microsoft.AspNetCore.Mvc;
using TRFSAE.MemberPortal.API.DTOs;
using TRFSAE.MemberPortal.API.Interfaces;
using TRFSAE.MemberPortal.API.Enums;

namespace TRFSAE.MemberPortal.API.Controllers;

[ApiController]
[Route("api/projects/columns")]
public class ProjectColumnController : ControllerBase
{
    private readonly IProjectColumnService _ProjectColumnService;
    public ProjectColumnController(IProjectColumnService projectService)
    {
        _ProjectColumnService = projectService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAllColumns([FromQuery] Guid projectId)
    {
        var projects = await _ProjectColumnService.GetAllColumnsAsync(projectId);
        return Ok(projects);
    }

    [HttpGet("fetch")]
    public async Task<IActionResult> GetColumnById([FromQuery] Guid projectId, [FromQuery] Guid id)
    {
        var projects = await _ProjectColumnService.GetColumnByIdAsync(projectId, id);
        return Ok(projects);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateColumn([FromQuery] Guid projectId, CreateColumnDto createDto)
    {
        var project = await _ProjectColumnService.CreateColumnAsync(projectId, createDto);
        return Ok(project);
    }

    [HttpPatch("update")]
    public async Task<IActionResult> UpdateColumn([FromQuery] Guid id, UpdateColumnDto updateDto)
    {
        var project = await _ProjectColumnService.UpdateColumnAsync(id, updateDto);
        return Ok(project);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteColumn([FromQuery] Guid id) // needs to be turned into RPC; return value is true as long as GUID is valid
    {
        var project = await _ProjectColumnService.DeleteColumnAsync(id);
        return Ok(project);
    }
}
