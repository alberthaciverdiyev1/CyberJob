using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberJob.Models;

[Table("filters")]
public class Filter
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name", TypeName = "json")]
    public string? Name { get; set; } 

    [Column("key")]
    public string? Key { get; set; } 

    [Column("parent_id")]
    public int? ParentId { get; set; }


    [ForeignKey("ParentId")]
    public virtual Filter? Parent { get; set; }

    public virtual ICollection<Filter> SubFilters { get; set; } = new List<Filter>();

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}