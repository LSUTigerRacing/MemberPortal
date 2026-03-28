using System.Text.Json.Serialization;
using TRFSAE.MemberPortal.API.Enums;

namespace TRFSAE.MemberPortal.API.DTOs;

public class UpdateColumnDto
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("color")]
    public int Color { get; set; }
}
