namespace CyberJob.DTOs;

public class SubscriptionPlanDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double OldPrice { get; set; }
    public double NewPrice { get; set; }
    public string? Type { get; set; }
    public List<SubscriptionPlanOptionDto> Options { get; set; } = new();
}

public class SubscriptionPlanOptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
