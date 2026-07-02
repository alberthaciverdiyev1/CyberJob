using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberJob.Models;

[Table("subscription_plan_options")]
public class SubscriptionPlanOption
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name", TypeName = "json")]
    public string? Name { get; set; }

    [Column("subscription_plan_id")]
    public int SubscriptionPlanId { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    public SubscriptionPlan? Plan { get; set; }
}
