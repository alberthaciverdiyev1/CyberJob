using CyberJob.Core.DTOs.Common;
using CyberJob.Core.DTOs.Partner;
using CyberJob.Core.Entities;

namespace CyberJob.Core.Services;

public interface IPartnerService : IGenericService<Partner, PartnerResponse>
{
    Task<ApiResponse<PartnerResponse>> AddAsync(CreatePartnerRequest request);
    Task<ApiResponse> UpdateAsync(UpdatePartnerRequest request);
}