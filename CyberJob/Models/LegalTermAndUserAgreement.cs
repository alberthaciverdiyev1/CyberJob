using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberJob.Models;

[Table("legal_term_and_user_agreements")]
public class LegalTermAndUserAgreement
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title", TypeName = "json")]
    public string? Title { get; set; }

    [Column("content", TypeName = "json")]
    public string? Content { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("type")]
    public string? Type { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}
