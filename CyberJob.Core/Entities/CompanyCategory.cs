namespace CyberJob.Core.Entities;

public class CompanyCategory : BaseEntity
{
    public string? Name { get; set; }
    
    
    public virtual ICollection<Company> Companies { get; set; } = new HashSet<Company>();
}