namespace CyberJob.Core.DTOs.Vacancy;

public record CreateVacancyRequest(
    string? Title,
    string? BannerImage,
    int CompanyId,
    int CategoryId,
    List<int>? FilterIds
);