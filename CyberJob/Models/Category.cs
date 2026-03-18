using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberJob.Models;

[Table("categories")]
public class Category
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name", TypeName = "json")]
    public string Name { get; set; } 

    [Column("icon")]
    public string? Icon { get; set; }

    [Column("parent_id")]
    public int? ParentId { get; set; }


    [ForeignKey("ParentId")]
    public virtual Category? Parent { get; set; }

    public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}