using TRFSAE.MemberPortal.API.DTOs;
using TRFSAE.MemberPortal.API.Interfaces;
using TRFSAE.MemberPortal.API.Models;
using Supabase;

namespace TRFSAE.MemberPortal.API.Services;

public class ProjectColumnService : IProjectColumnService
{
    private readonly Client _supabaseClient;

    public ProjectColumnService(Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<IEnumerable<ColumnResponseDto>> GetAllColumnsAsync(int projectId)
    {
        var response = await _supabaseClient
            .From<ProjectColumnModel>()
            .Select("*")
            .Get();

        var columnResponses = response.Models.Select(p => new ColumnResponseDto
        {
            Id = p.Id,
            ProjectId = projectId,
            Title = p.Title,
            Color = p.Color
        });

        return columnResponses;
    }

    public async Task<ColumnResponseDto> GetColumnByIdAsync(int projectId, Guid id)
    {
        var response = await _supabaseClient
            .From<ProjectColumnModel>()
            .Where(p => p.Id == id)
            .Select("*")
            .Single();

        var columnResponses = new ColumnResponseDto
        {
            Id = response.Id,
            ProjectId = projectId,
            Title = response.Title,
            Color = response.Color
        };

        return columnResponses;
    }

    public async Task<bool> CreateColumnAsync(int projectId, CreateColumnDto createDto)
    {
        var columnId = Guid.NewGuid();

        var newColumn = new ProjectColumnModel
        {
            Id = columnId,
            ProjectId = projectId,
            Title = createDto.Title,
            Color = createDto.Color
        };

        try
        {
            var response = await _supabaseClient
                .From<ProjectColumnModel>()
                .Insert(newColumn);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error attempting insert: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateColumnAsync(Guid id, UpdateColumnDto updateDto)
    {
        try
        {
            var model = await _supabaseClient.From<ProjectColumnModel>()
                .Where(p => p.Id == id)
                .Single();

            if (model == null)
            {
                Console.WriteLine("Column not found");
                return false;
            }

            if (!string.IsNullOrEmpty(updateDto.Title))
            {
                model.Title = updateDto.Title;
            }
            if (updateDto.Color != 0)
            {
                model.Color = updateDto.Color;
            }

            var response = await _supabaseClient
                .From<ProjectColumnModel>()
                .Update(model);

            return response.Models.Count > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error attempting update: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteColumnAsync(Guid id)
    {
        try
        {
            await _supabaseClient
            .From<ProjectColumnModel>()
            .Where(x => x.Id == id)
            .Delete();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Delete Failed: {e.Message}");
            return false;
        }    }
}
