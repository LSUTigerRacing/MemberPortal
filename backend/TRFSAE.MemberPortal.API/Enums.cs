using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TRFSAE.MemberPortal.API.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum TRSystem
{
    Chassis,
    Powertrain,
    Software,
    Business
}

[JsonConverter(typeof(StringEnumConverter))]
public enum Subsystem
{
    // Chassis
    Frame,
    Aerodynamics,
    Ergonomics,
    Brakes,
    Suspension,

    // Powertrain
    Battery,
    Electronics,
    LowVoltage,
    TractiveSystem,

    // Software
    App,
    Embedded,
    Data,

    // Business
    Finance,
    PublicRelations
}

[JsonConverter(typeof(StringEnumConverter))]
public enum ShirtSize
{
    XS,
    S,
    M,
    L,
    XL,
    XXL
}

// EnumMember values must match the Postgres `roles` enum strings exactly
// (shared/config/enums.ts is the source of truth). Two separate JSON
// stacks touch this enum and each ignores the other's attribute:
// Newtonsoft.Json (used internally by Supabase.Postgrest for the `role`
// DB column) reads [EnumMember]; System.Text.Json (ASP.NET Core's default
// controller response serializer — e.g. AuthController.Me()) reads
// [JsonStringEnumMemberName] instead. Confirmed empirically that without
// both, this enum silently serializes as a raw integer (e.g. `3`) on
// whichever boundary is missing its attribute, breaking any frontend code
// that compares the value against the Role string enum.
[JsonConverter(typeof(StringEnumConverter))]
[System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
public enum Role
{
    [EnumMember(Value = "Superadmin")]
    [System.Text.Json.Serialization.JsonStringEnumMemberName("Superadmin")]
    SuperAdmin,
    [EnumMember(Value = "Admin")]
    Admin,
    [EnumMember(Value = "System Lead")]
    [System.Text.Json.Serialization.JsonStringEnumMemberName("System Lead")]
    SystemLead,
    [EnumMember(Value = "Subsystem Lead")]
    [System.Text.Json.Serialization.JsonStringEnumMemberName("Subsystem Lead")]
    SubsystemLead,
    [EnumMember(Value = "Member")]
    Member,
    [EnumMember(Value = "Unverified")]
    Unverified
}

[JsonConverter(typeof(StringEnumConverter))]
public enum ProjectPriority
{
    Low,
    Medium,
    High
}

[JsonConverter(typeof(StringEnumConverter))]
public enum ProjectStatus
{
    Draft,
    Active,
    OnHold,
    Completed
}

[JsonConverter(typeof(StringEnumConverter))]
public enum OrderStatus
{
    Pending,
    Denied,
    Approved,
    Delivering,
    Delivered,
    Claimed
}
