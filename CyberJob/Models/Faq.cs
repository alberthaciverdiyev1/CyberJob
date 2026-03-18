using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberJob.Models;

[Table("faqs")]
public class Faq
{   [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("question", TypeName = "json")]
    public string? Question { get; set; }
    [Column("answer", TypeName = "json")]
    public string? Answer { get; set; }
    
    [Column("created_at")] 
    public DateTime CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }


}