using CyberJob.DTOs;

namespace CyberJob.ViewModels;

public class ServicesVM
{
    public List<FaqDto> Faqs { get; set; } = new List<FaqDto>();
    public List<SubscriptionPlanDto> Plans { get; set; } = new List<SubscriptionPlanDto>();
}