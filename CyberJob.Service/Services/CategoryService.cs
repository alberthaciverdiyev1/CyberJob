using System.Net;
using AutoMapper;
using CyberJob.Core.DTOs.Category;
using CyberJob.Core.DTOs.Common;
using CyberJob.Core.Entities;
using CyberJob.Core.Repositories;
using CyberJob.Core.Services;

namespace CyberJob.Service.Services;

public class CategoryService(IGenericRepository<Category> repository, IUnitOfWork unitOfWork, IMapper mapper)
    : GenericService<Category, CategoryResponse>(repository, unitOfWork, mapper), ICategoryService
{
    public async Task<ApiResponse<CategoryResponse>> AddAsync(CreateCategoryRequest request)
    {
        var category = Mapper.Map<Category>(request);
        await Repository.AddAsync(category);

        await UnitOfWork.CommitAsync();

        var data = Mapper.Map<CategoryResponse>(category);
        return ApiResponse<CategoryResponse>.Success(HttpStatusCode.Created, data, "Category Created Successfully");
    }

    public async Task<ApiResponse> UpdateAsync(UpdateCategoryRequest dto)
    {
        var category = Mapper.Map<Category>(dto);

        Repository.Update(category);
        await UnitOfWork.CommitAsync();
        return ApiResponse.Success(HttpStatusCode.NoContent, "Category Updated successfully");
    }

    public async Task UpdateWithCheckAsync(int id, UpdateCategoryRequest dto)
    {
        var existCategory = await Repository.GetByIdAsync(id);
        if (existCategory == null)
        {
            throw new Exception("Category not found");
        }

        Mapper.Map(dto, existCategory);

        Repository.Update(existCategory);
        await UnitOfWork.CommitAsync();
    }
}