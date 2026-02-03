namespace CyberJob.Core.DTOs.Vacancy;

public record VacancyResponse(
    int Id,
    string Title,
    int ViewCount,
    bool IsPremium,
    bool IsPromoted,
    int CompanyName,
    int CompanyAddress,
    DateTime CreatedAt
);