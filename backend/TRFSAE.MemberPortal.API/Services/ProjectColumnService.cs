using TRFSAE.MemberPortal.API.DTOs;
using TRFSAE.MemberPortal.API.Interfaces;
using Supabase;

namespace TRFSAE.MemberPortal.API.Services;

public class ProjectColumnService : IProjectColumnService
{
    private readonly Client _supabaseClient;

    public ProjectColumnService(Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public Task<bool> CreateColumnAsync(CreateProjectDto createDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteColumnAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ProjectSummaryDto>> GetAllColumnsAsync(Guid projectId)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectDetailDto> GetColumnByIdAsync(Guid projectId, Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateColumnAsync(Guid id, UpdateProjectDto updateDto)
    {
        throw new NotImplementedException();
    }
}
