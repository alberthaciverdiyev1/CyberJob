using CyberJob.DTOs;

namespace CyberJob.ViewModels;

public class RankIndexVM
{
    public IEnumerable<FaqDto> Faqs { get; set; } = Enumerable.Empty<FaqDto>();
    public IEnumerable<CompanyListDto> Companies { get; set; } = Enumerable.Empty<CompanyListDto>();
    public int TotalCount { get; set; }
}