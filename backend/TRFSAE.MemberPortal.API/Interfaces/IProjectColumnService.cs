using TRFSAE.MemberPortal.API.DTOs;

namespace TRFSAE.MemberPortal.API.Interfaces;

public interface IProjectColumnService
{
    Task<IEnumerable<ProjectSummaryDto>> GetAllColumnsAsync(Guid projectId);
    Task<ProjectDetailDto> GetColumnByIdAsync(Guid projectId, Guid id);
    Task<bool> CreateColumnAsync(CreateProjectDto createDto);
    Task<bool> UpdateColumnAsync(Guid id, UpdateProjectDto updateDto);
    Task<bool> DeleteColumnAsync(Guid id);
}
