namespace CyberJob.Core.DTOs.Vacancy;

public record VacancyDetailsResponse(
    int Id,
    string? Title,
    string? Description,
    string? BannerImage,
    bool IsPremium,
    bool IsPromoted,
    DateTime ExpirationDate,
    DateTime CreatedAt,
    
    int CompanyId,
    string? CompanyName,
    string? CompanyLogo, 
    
    int? CategoryId,
    string? CategoryName
);