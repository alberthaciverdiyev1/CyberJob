namespace CyberJob.Core.DTOs.Vacancy;

public record UpdateVacancyRequest(
    int Id,
    string? Title,
    string? BannerImage,
    int CompanyId,
    int CategoryId,
    DateTime ExpirationDate,
    bool IsPremium,
    bool IsPromoted
);