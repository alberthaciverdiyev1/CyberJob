using System.Net;
using AutoMapper;
using CyberJob.Core.DTOs.Common;
using CyberJob.Core.DTOs.Filter;
using CyberJob.Core.Entities;
using CyberJob.Core.Repositories;
using CyberJob.Core.Services;

namespace CyberJob.Service.Services;

public class FilterService(IGenericRepository<Filter> repository, IUnitOfWork unitOfWork, IMapper mapper)
    : GenericService<Filter, FilterResponse>(repository, unitOfWork, mapper), IFilterService
{
    public async Task<ApiResponse> AddAsync(CreateFilterRequest request)
    {
        var entity = Mapper.Map<Filter>(request);
        await Repository.AddAsync(entity);
        await UnitOfWork.CommitAsync();
        return ApiResponse.Success(HttpStatusCode.Created, "Filter Created Successfully");
    }

    public async Task<ApiResponse> UpdateAsync(UpdateFilterRequest request)
    {
        var entity = await Repository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            return ApiResponse.Fail(HttpStatusCode.NotFound, "Filter Not Found");
        }

        Mapper.Map(request, entity);
        await UnitOfWork.CommitAsync();
        return ApiResponse.Success(HttpStatusCode.NoContent, "Filter Updated Successfully");
    }
}