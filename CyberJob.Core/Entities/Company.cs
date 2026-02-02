namespace CyberJob.Core.Entities;

public class Company : BaseEntity
{
    public string? Image { get; set; }
    public string? BannerImage { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? ShortAddress { get; set; }
    public bool IsActive { get; set; }
    public bool IsVerified { get; set; }
    public DateOnly FoundingDate { get; set; }
    public string? About { get; set; }
    
    public int CategoryId { get; set; } 
    public virtual CompanyCategory? Category { get; set; }
    
}