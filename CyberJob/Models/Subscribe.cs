using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberJob.Models;

[Table("subscribes")]
public class Subscribe
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("email")]
    public string Email { get; set; } 


    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}