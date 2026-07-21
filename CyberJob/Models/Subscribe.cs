using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberJob.Models;

[Table("subscribes")]
public class Subscribe
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Email daxil edin.")]
    [EmailAddress(ErrorMessage = "Düzgün email ünvanı daxil edin.")]
    [MaxLength(200)]
    [Column("email")]
    public string Email { get; set; } = string.Empty;


    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}