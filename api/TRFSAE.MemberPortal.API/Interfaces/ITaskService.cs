using System.Text.Json;
using TRFSAE.MemberPortal.API.DTOs;

namespace TRFSAE.MemberPortal.API.Interfaces;

public interface ITaskService
{
    Task<List<TaskDetailDto>> GetAllTasksAsync(Guid columnId);
    Task<TaskDetailDto> GetTasksByIdAsync(Guid id);
    Task<bool> CreateTaskAsync(int projectId, Guid columnId, CreateTaskDto createDto);
    Task<bool> UpdateTaskAsync(Guid id, UpdateTaskDto updateDto);
    Task<bool> DeleteTaskAsync(Guid id);
}
