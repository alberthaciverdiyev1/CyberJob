namespace CyberJob.Core.Entities;

public class Vacancy : BaseEntity
{
    public string? Title { get; set; }
    public string? BannerImage { get; set; }

    public int CompanyId { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsPremium { get; set; }
    public bool IsPromoted { get; set; }
    public virtual Company? Company { get; set; }
    public virtual ICollection<VacancyFilter> VacancyFilters { get; set; } = new HashSet<VacancyFilter>();
}