using TRFSAE.MemberPortal.API.Models;
using TRFSAE.MemberPortal.API.Interfaces;
using TRFSAE.MemberPortal.API.DTOs;
using System.Text.Json;
using Supabase;

namespace TRFSAE.MemberPortal.API.Services;

public class TaskService : ITaskService
{
    private readonly Client _supabaseClient;
    public TaskService(Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }
 
    public async Task<List<TaskDetailDto>> GetAllTasksAsync(Guid columnId)
    {
        var response = await _supabaseClient
        .From<ProjectTaskModel>()
        .Where(x => x.ColumnId == columnId)
        .Get();

        return response.Models.Select(task => new TaskDetailDto
        {
            Id = task.Id,
            ProjectId = task.ProjectId,
            AuthorId = task.AuthorId,
            Title = task.Title,
            Description = task.Description,
            Priority = task.Priority,
            Deadline = task.Deadline,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        }).ToList();
    }

    public async Task<TaskDetailDto> GetTasksByIdAsync(Guid id)
    {
        var response = await _supabaseClient
        .From<ProjectTaskModel>()
        .Where(x => x.Id == id)
        .Single();

        if (response == null)
        {
            Console.WriteLine("Task not found");
            return null;
        }

        var taskDetail = new TaskDetailDto
        {
            Id = response.Id,
            ProjectId = response.ProjectId,
            AuthorId = response.AuthorId,
            Title = response.Title,
            Description = response.Description,
            Priority = response.Priority,
            Deadline = response.Deadline,
            CreatedAt = response.CreatedAt,
            UpdatedAt = response.UpdatedAt
        };

        return taskDetail;
    }

    public async Task<bool> CreateTaskAsync(int projectId, Guid columnId, CreateTaskDto createDto)
    {
        var newTask = new ProjectTaskModel
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            ColumnId = columnId,
            AuthorId = new Guid("f1ea4117-d20b-4e9b-a913-b3fa3180b101"), // This should be replaced with the actual author ID from the context
            Title = createDto.Title,
            Description = createDto.Description,
            Priority = createDto.Priority,
            Deadline = createDto.Deadline,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        try
        {
            var response = await _supabaseClient
            .From<ProjectTaskModel>()
            .Insert(newTask);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating task: {ex.Message}");
            return false;
        }

        return true;
    }

    public async Task<bool> UpdateTaskAsync(Guid id, UpdateTaskDto updateDto)
    {
        try
        {
            var model = await _supabaseClient.From<ProjectTaskModel>()
                .Where(p => p.Id == id)
                .Single();

            if (model == null)
            {
                Console.WriteLine("Project not found");
                return false;
            }

            if (!string.IsNullOrEmpty(updateDto.Title))
            {
                model.Title = updateDto.Title;
            }
            if (!string.IsNullOrEmpty(updateDto.Description))
            {
                model.Description = updateDto.Description;
            }
            if (updateDto.Deadline != null)
            {
                model.Deadline = updateDto.Deadline.Value;
            }
            model.UpdatedAt = DateTime.UtcNow;

            var response = await _supabaseClient
                .From<ProjectTaskModel>()
                .Update(model);

            return response.Models.Count > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update task: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        var toDelete = new ProjectTaskModel
        {
            Id = id
        };

        var response = await _supabaseClient
        .From<ProjectTaskModel>()
        .Delete(toDelete);

        var deletedTask = response.Model;

        if (deletedTask == null)
        {
            Console.WriteLine("Task not found or could not be deleted");
            return false;
        }

        return true;
    }
}
