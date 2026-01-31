using CyberJob.Core.DTOs.BannerDto;
using CyberJob.Core.DTOs.Common;
using CyberJob.Core.Entities;

namespace CyberJob.Core.Services;

public interface IBannerService : IGenericService<Banner, BannerResponse>
{
    Task<ApiResponse<BannerResponse>> AddAsync(BannerCreateRequest dto);
    Task<ApiResponse> UpdateAsync(BannerUpdateRequest dto);
    Task<ApiResponse<IEnumerable<BannerResponse>>> GetBannersByPage(string type);
    
}