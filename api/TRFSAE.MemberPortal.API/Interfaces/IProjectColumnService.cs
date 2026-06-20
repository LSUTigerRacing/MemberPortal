using TRFSAE.MemberPortal.API.DTOs;

namespace TRFSAE.MemberPortal.API.Interfaces;

public interface IProjectColumnService
{
    Task<IEnumerable<ColumnResponseDto>> GetAllColumnsAsync(int projectId);
    Task<ColumnResponseDto> GetColumnByIdAsync(int projectId, Guid id);
    Task<bool> CreateColumnAsync(int projectId, CreateColumnDto createDto);
    Task<bool> UpdateColumnAsync(Guid id, UpdateColumnDto updateDto);
    Task<bool> DeleteColumnAsync(Guid id);
}
