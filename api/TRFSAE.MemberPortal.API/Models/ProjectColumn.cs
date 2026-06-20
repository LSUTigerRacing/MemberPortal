using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using TRFSAE.MemberPortal.API.Enums;

namespace TRFSAE.MemberPortal.API.Models;

[Table("project_column")]
public class ProjectColumnModel : BaseModel
{
    [PrimaryKey("id", false)]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("projectId")]
    public int ProjectId { get; set; }

    [Column("title")]
    public string Title { get; set; } = "Untitled";

    [Column("color")]
    public int Color { get; set; }

}
