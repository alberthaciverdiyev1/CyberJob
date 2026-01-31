namespace CyberJob.Core.DTOs.BannerDto;

public record BannerUpdateRequest(int Id, string Type, string Page, DateTime ExpirationDate, Stream ImageFile);