using CyberJob.Core.DTOs.Common;
using CyberJob.Core.DTOs.CompanyCategory;
using CyberJob.Core.Entities;

namespace CyberJob.Core.Services;

public interface ICompanyCategoryService:IGenericService<CompanyCategory,CompanyCategoryResponse>
{
    Task<ApiResponse<CompanyCategoryResponse>> AddAsync(CreateCompanyCategoryRequest request);
    Task<ApiResponse> UpdateAsync(UpdateCompanyCategoryRequest request);
}