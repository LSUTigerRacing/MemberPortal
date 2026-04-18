using TRFSAE.MemberPortal.API.Enums;

namespace TRFSAE.MemberPortal.API.DTOs;

public class CreateTaskDto
{
    public string Title { get; set; } = null!;
    public ProjectPriority Priority { get; set; }
    public string? Description { get; set; }
    // public Guid[]? Assignees { get; set; }
    public DateTime Deadline { get; set; }
}
