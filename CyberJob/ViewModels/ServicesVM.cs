using CyberJob.DTOs;

namespace CyberJob.ViewModels;

public class ServicesVM
{
    public List<FaqDto> Faqs { get; set; } = new List<FaqDto>();
}