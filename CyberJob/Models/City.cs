using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberJob.Models;

[Table("cities")]
public class City
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name", TypeName = "json")]
    public string Name { get; set; } 


    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}