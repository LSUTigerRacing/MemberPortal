using TRFSAE.MemberPortal.API.DTOs;

namespace TRFSAE.MemberPortal.API.Interfaces;

public interface IProjectColumnService
{
    Task<IEnumerable<ColumnResponseDto>> GetAllColumnsAsync(Guid projectId);
    Task<ColumnResponseDto> GetColumnByIdAsync(Guid projectId, Guid id);
    Task<bool> CreateColumnAsync(Guid projectId, CreateColumnDto createDto);
    Task<bool> UpdateColumnAsync(Guid id, UpdateColumnDto updateDto);
    Task<bool> DeleteColumnAsync(Guid id);
}
