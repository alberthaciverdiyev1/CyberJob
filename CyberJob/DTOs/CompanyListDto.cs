namespace CyberJob.DTOs;

public class CompanyListDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Logo { get; set; }
    public int? VacancyCount { get; set; }
        
    public string? Email { get; set; }
    public bool IsVerified { get; set; }
    public string? BannerImage { get; set; }
}