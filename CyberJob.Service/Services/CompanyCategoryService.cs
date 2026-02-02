using System.Net;
using AutoMapper;
using CyberJob.Core.DTOs.Common;
using CyberJob.Core.DTOs.CompanyCategory;
using CyberJob.Core.Entities;
using CyberJob.Core.Repositories;
using CyberJob.Core.Services;

namespace CyberJob.Service.Services;

public class CompanyCategoryService(IGenericRepository<CompanyCategory> repository, IUnitOfWork unitOfWork, IMapper mapper) :GenericService<CompanyCategory,CompanyCategoryResponse>(repository, unitOfWork, mapper),ICompanyCategoryService
{
    public async Task<ApiResponse<CompanyCategoryResponse>> AddAsync(CreateCompanyCategoryRequest request)
    {
        var entity = Mapper.Map<CompanyCategory>(request);
        await Repository.AddAsync(entity);
        await UnitOfWork.CommitAsync();
        return ApiResponse<CompanyCategoryResponse>.Success(HttpStatusCode.Created,
            Mapper.Map<CompanyCategoryResponse>(entity), "Company Category Created Successfully");
    }

    public async Task<ApiResponse> UpdateAsync(UpdateCompanyCategoryRequest request)
    {
        var existingCompanyCategory = await Repository.GetByIdAsync(request.Id);

        if (existingCompanyCategory is null)
        {
            return ApiResponse.Fail(HttpStatusCode.NotFound, "Company Category Not Found For Edit");
        }
        
        var entity = Mapper.Map(request, existingCompanyCategory);
        
        Repository.Update(entity);
        await UnitOfWork.CommitAsync();
        
        return ApiResponse.Success(HttpStatusCode.NoContent, "Company Category Updated Successfully");
    }
}