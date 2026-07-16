namespace CyberJob.DTOs;

public class SubscriptionPlanDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double OldPrice { get; set; }
    public double NewPrice { get; set; }
    public string? Type { get; set; }
    public DateTime? DiscountStart { get; set; }
    public DateTime? DiscountEnd { get; set; }
    public bool IsPremium { get; set; }
    public List<SubscriptionPlanOptionDto> Options { get; set; } = new();

    public bool HasActiveDiscount =>
        DiscountStart != null && DiscountEnd != null &&
        DiscountStart.Value.Date <= DateTime.UtcNow.Date &&
        DiscountEnd.Value.Date >= DateTime.UtcNow.Date;
}

public class SubscriptionPlanOptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
