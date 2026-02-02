using System.Net;
using AutoMapper;
using CyberJob.Core.DTOs.Common;
using CyberJob.Core.DTOs.Partner;
using CyberJob.Core.Entities;
using CyberJob.Core.Repositories;
using CyberJob.Core.Services;

namespace CyberJob.Service.Services;

public class PartnerService(IGenericRepository<Partner> repository, IUnitOfWork unitOfWork, IMapper mapper)
    : GenericService<Partner, PartnerResponse>(repository, unitOfWork, mapper), IPartnerService
{
    public async Task<ApiResponse<PartnerResponse>> AddAsync(CreatePartnerRequest request)
    {
        var partner = Mapper.Map<CreatePartnerRequest, Partner>(request);

        await Repository.AddAsync(partner);
        await UnitOfWork.CommitAsync();

        var data = Mapper.Map<PartnerResponse>(partner);

        return ApiResponse<PartnerResponse>.Success(HttpStatusCode.Created, data, "Partner Created Successfully");
    }

    public async Task<ApiResponse> UpdateAsync(UpdatePartnerRequest request)
    {
        var existingPartner = await Repository.GetByIdAsync(request.Id);
        
        if (existingPartner is null)
        {
            return ApiResponse.Fail(HttpStatusCode.NotFound, "Partner Not Found");
        }

        Mapper.Map(request, existingPartner);
        
        Repository.Update(existingPartner);
        await UnitOfWork.CommitAsync();

        return ApiResponse.Success(HttpStatusCode.OK, "Partner Updated Successfully");
    }
}