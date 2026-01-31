namespace CyberJob.Core.Entities;

public class Banner : BaseEntity 
{
    public string ImageUrl { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Page { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }
}