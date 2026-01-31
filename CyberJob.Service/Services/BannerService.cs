using System.Net;
using AutoMapper;
using CyberJob.Core.DTOs.BannerDto;
using CyberJob.Core.DTOs.Common;
using CyberJob.Core.Entities;
using CyberJob.Core.Repositories;
using CyberJob.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Service.Services;

public class BannerService(IBannerRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
    : GenericService<Banner, BannerResponse>(repository, unitOfWork, mapper), IBannerService
{
    public async Task<ApiResponse<BannerResponse>> AddAsync(BannerCreateRequest request)
    {
        var entity = Mapper.Map<Banner>(request);
        await Repository.AddAsync(entity);
        await UnitOfWork.CommitAsync();

        var data = Mapper.Map<BannerResponse>(entity);

        return ApiResponse<BannerResponse>.Success(HttpStatusCode.Created, data, "Banner Added Successfully");
    }

    public async Task<ApiResponse> UpdateAsync(BannerUpdateRequest request)
    {
        var entity = Mapper.Map<Banner>(request);
        Repository.Update(entity);
        await UnitOfWork.CommitAsync();

        return ApiResponse.Success(HttpStatusCode.NoContent, "Banner Updated Successfully");
    }

    public async Task<ApiResponse<IEnumerable<BannerResponse>>> GetBannersByPage(string type)
    {
        var bannersQuery = repository.GetBannersByPage(type);

        var banners = await bannersQuery.ToListAsync();

        var data = Mapper.Map<IEnumerable<BannerResponse>>(banners);
        return ApiResponse<IEnumerable<BannerResponse>>.Success(HttpStatusCode.OK, data,
            "Banner Retrieved Successfully");
    }
}