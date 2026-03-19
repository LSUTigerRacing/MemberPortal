using TRFSAE.MemberPortal.API.Models;
using TRFSAE.MemberPortal.API.Enums;
using System.Text.Json.Serialization;

namespace TRFSAE.MemberPortal.API.DTOs;

public class OrderCreateDto
{
    public string Name { get; set; } = null!;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Subsystem Subsystem { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime Deadline { get; set; }
    public string? Notes { get; set; }
}

