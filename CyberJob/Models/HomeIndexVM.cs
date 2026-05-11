using CyberJob.DTOs;

namespace CyberJob.Models;

public class HomeIndexVM
{
    public IEnumerable<BannerDto> Banners { get; set; } = Enumerable.Empty<BannerDto>();
    public IEnumerable<CategoryDto> Categories { get; set; } = Enumerable.Empty<CategoryDto>();
    public IEnumerable<PartnerDto> Partners { get; set; } = Enumerable.Empty<PartnerDto>();
    public IEnumerable<VacancyListDto> PremiumVacancies { get; set; } = Enumerable.Empty<VacancyListDto>();
    public IEnumerable<VacancyListDto> LatestVacancies { get; set; } = Enumerable.Empty<VacancyListDto>();

    public int VisitorDaily { get; set; }
    public int VisitorWeekly { get; set; }
    public int VisitorMonthly { get; set; }
    public int VisitorTotal { get; set; }

    public int VacancyDaily { get; set; }
    public int VacancyWeekly { get; set; }
    public int VacancyMonthly { get; set; }
    public int VacancyTotal { get; set; }
}
