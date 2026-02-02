namespace CyberJob.Core.Entities;

public class Filter : BaseEntity
{
    public string? Key { get; set; }
    public string? Name { get; set; }
    
    public virtual ICollection<VacancyFilter> VacancyFilters { get; set; } = new HashSet<VacancyFilter>();
}