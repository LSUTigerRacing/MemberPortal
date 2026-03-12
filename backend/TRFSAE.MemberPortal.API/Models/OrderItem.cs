using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace TRFSAE.MemberPortal.API.Models;

[Table("order_item")]
public class OrderItemModel : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("orderId")]
    public Guid OrderId { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("partNumber")]
    public string PartNumber { get; set; }

    [Column("supplier")]
    public string Supplier { get; set; }

    [Column("url")]
    public string URL { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
