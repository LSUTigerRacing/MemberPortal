using TRFSAE.MemberPortal.API.Enums;

namespace TRFSAE.MemberPortal.API.DTOs;

public class UpdateTaskDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ProjectStatus? Status { get; set; }
    public DateTime? Deadline { get; set; }

}
