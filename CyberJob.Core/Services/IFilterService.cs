using CyberJob.Core.DTOs.Common;
using CyberJob.Core.DTOs.Filter;
using CyberJob.Core.Entities;

namespace CyberJob.Core.Services;

public interface IFilterService : IGenericService<Filter, FilterResponse>
{
    public Task<ApiResponse> AddAsync(CreateFilterRequest request);
    public Task<ApiResponse> UpdateAsync(UpdateFilterRequest request);
}