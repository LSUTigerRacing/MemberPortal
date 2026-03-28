using System.Text.Json.Serialization;
using TRFSAE.MemberPortal.API.Enums;

namespace TRFSAE.MemberPortal.API.DTOs;

public class ColumnResponseDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("projectId")]
    public Guid ProjectId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("color")]
    public int Color { get; set; }
}
