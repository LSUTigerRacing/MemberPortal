using TRFSAE.MemberPortal.API.DTOs;
using TRFSAE.MemberPortal.API.Enums;

namespace TRFSAE.MemberPortal.API.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectSummaryDto>> GetAllProjectsAsync();
    Task<ProjectDetailDto> GetProjectByIdAsync(int id);
    Task<bool> CreateProjectAsync(CreateProjectDto createDto);
    Task<bool> UpdateProjectAsync(int id, UpdateProjectDto updateDto);
    Task<bool> DeleteProjectAsync(int id);
}
