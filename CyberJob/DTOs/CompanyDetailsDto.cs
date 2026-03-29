namespace CyberJob.DTOs;

public class CompanyDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? ShortAddress { get; set; }
    public int? FoundedYear { get; set; }

    public string? About { get; set; }
    public string? Logo { get; set; }
    public string? CoverImage { get; set; }
    public string? BannerImage { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? CompanyCategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public int VacancyCount { get; set; }
    public List<VacancyListDto> Vacancies { get; set; } = new List<VacancyListDto>();
}