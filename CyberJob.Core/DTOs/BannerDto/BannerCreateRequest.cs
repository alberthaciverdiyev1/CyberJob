using System.Net.Mime;
namespace CyberJob.Core.DTOs.BannerDto;

public record BannerCreateRequest(string Type, string Page, DateTime ExpirationDate,Stream ImageFile);