using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace TRFSAE.MemberPortal.API.Models;

[Table("order_review")]
public class OrderReviewModel : BaseModel
{
    [PrimaryKey("userId")]
    public Guid UserId { get; set; }

    [PrimaryKey("orderId")]
    public Guid OrderId { get; set; }

    [Column("value")]
    public bool Value { get; set; }
    
    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }
}
