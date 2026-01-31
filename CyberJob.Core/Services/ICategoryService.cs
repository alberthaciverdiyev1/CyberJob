using CyberJob.Core.DTOs.Category;
using CyberJob.Core.DTOs.Common;
using CyberJob.Core.Entities;

namespace CyberJob.Core.Services;

public interface ICategoryService:IGenericService<Category, CategoryResponse>
{
    Task<ApiResponse<CategoryResponse>> AddAsync(CreateCategoryRequest dto);
    Task<ApiResponse> UpdateAsync(UpdateCategoryRequest dto);
}