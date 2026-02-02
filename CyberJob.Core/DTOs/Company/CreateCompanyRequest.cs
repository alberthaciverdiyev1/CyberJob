namespace CyberJob.Core.DTOs.Company;

public record CreateCompanyRequest(
    string? Image,
    string? BannerImage,
    string Name,
    string? Email,
    string? Phone,
    string? Address,
    string? ShortAddress,
    bool? IsActive,
    bool? IsVerified,
    DateOnly? FoundingDate,
    string? About,
    int? CategoryId
);