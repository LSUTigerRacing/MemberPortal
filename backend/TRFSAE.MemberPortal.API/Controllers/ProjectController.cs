using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRFSAE.MemberPortal.API.DTOs;
using TRFSAE.MemberPortal.API.Interfaces;
using TRFSAE.MemberPortal.API.Enums;

namespace TRFSAE.MemberPortal.API.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _ProjectService;
    private readonly IPermissionService _permissionService;

    public ProjectController(IProjectService projectService, IPermissionService permissionService)
    {
        _ProjectService = projectService;
        _permissionService = permissionService;
    }

    [HttpGet("list")]
    [Authorize(Policy = "MemberAA")]
    public async Task<IActionResult> GetAllProjects()
    {
        var projects = await _ProjectService.GetAllProjectsAsync();
        return Ok(projects);
    }

    [HttpGet("fetch")]
    [Authorize(Policy = "MemberAA")]
    public async Task<IActionResult> GetProjectById([FromQuery] int id)
    {
        var projects = await _ProjectService.GetProjectByIdAsync(id);
        return Ok(projects);
    }

    [HttpPost("create")]
    [Authorize(Policy = "SubsystemLeadAA")]
    public async Task<IActionResult> CreateProject(CreateProjectDto createDto)
    {
        var authorId = _permissionService.CurrentUserId ?? Guid.Empty;
        var project = await _ProjectService.CreateProjectAsync(createDto, authorId);
        return Ok(project);
    }

    [HttpPatch("update")]
    [Authorize(Policy = "SubsystemLeadAA")]
    public async Task<IActionResult> UpdateProject([FromQuery] int id, UpdateProjectDto updateDto)
    {
        var project = await _ProjectService.UpdateProjectAsync(id, updateDto);
        return Ok(project);
    }

    [HttpDelete("delete")]
    // Delete/Archive project: SubsystemLead AA, or the project's own creator
    // regardless of level — checked inline below since it's resource-scoped.
    public async Task<IActionResult> DeleteProject([FromQuery] int id) // needs to be turned into RPC; return value is true as long as the id is valid
    {
        ProjectDetailDto existing;
        try
        {
            existing = await _ProjectService.GetProjectByIdAsync(id);
        }
        catch (Exception)
        {
            return NotFound();
        }

        if (!_permissionService.CanDeleteProject(existing.AuthorId))
        {
            return Forbid();
        }

        var project = await _ProjectService.DeleteProjectAsync(id);
        return Ok(project);
    }
}
