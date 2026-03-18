using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberJob.Models;
[Table("partners")]

public class Partner
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("image")]
    public string Image { get; set; }
    [Column("link")]
    public string? Link { get; set; }
    [Column("is_active")]
    public bool IsActive { get; set; }
    [Column("created_at")]  
    public DateTime CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("deleted_at")] 
    public DateTime? DeletedAt { get; set; }
}