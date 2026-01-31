namespace CyberJob.Core.DTOs.BannerDto;

public record BannerResponse(int Id, string Type, string Page, Stream ImageFile, DateTime ExpirationDate,DateTime CreatedAt );