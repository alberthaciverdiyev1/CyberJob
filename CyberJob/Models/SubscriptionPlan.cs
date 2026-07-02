using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberJob.Models;

[Table("subscription_plans")]
public class SubscriptionPlan
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("old_price")]
    public double OldPrice { get; set; }

    [Column("new_price")]
    public double NewPrice { get; set; }

    [Column("name", TypeName = "json")]
    public string? Name { get; set; }

    [Column("type")]
    public string? Type { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    public List<SubscriptionPlanOption> Options { get; set; } = new();
}
