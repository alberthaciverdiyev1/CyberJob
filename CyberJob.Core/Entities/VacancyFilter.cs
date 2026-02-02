namespace CyberJob.Core.Entities;

public class VacancyFilter : BaseEntity
{
    public int VacancyId { get; set; }
    public virtual Vacancy? Vacancy { get; set; }

    public int FilterId { get; set; }
    public virtual Filter? Filter { get; set; }
    
}