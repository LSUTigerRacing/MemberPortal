using TRFSAE.MemberPortal.API.Models;
using TRFSAE.MemberPortal.API.Enums;
using System.Text.Json.Serialization;

namespace TRFSAE.MemberPortal.API.DTOs;

public class OrderUpdateDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; } = null!;

    [JsonPropertyName("subsystem")]
    public Subsystem? Subsystem { get; set; }

    [JsonPropertyName("status")]
    public OrderStatus? Status { get; set; }

    [JsonPropertyName("deadline")]
    public DateTime? Deadline { get; set; }

    [JsonPropertyName("notes")]
    public string? Notes { get; set; }
}
