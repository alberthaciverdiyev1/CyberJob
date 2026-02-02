using System.Net;
using AutoMapper;
using CyberJob.Core.DTOs.Common;
using CyberJob.Core.DTOs.Company;
using CyberJob.Core.Entities;
using CyberJob.Core.Repositories;
using CyberJob.Core.Services;

namespace CyberJob.Service.Services;

public class CompanyService(IGenericRepository<Company> repository, IUnitOfWork unitOfWork, IMapper mapper) :GenericService<Company,CompanyResponse>(repository, unitOfWork, mapper),ICompanyService
{
    public async Task<ApiResponse<CompanyResponse>> AddAsync(CreateCompanyRequest request)
    {
       var entity = Mapper.Map<Company>(request);
       await Repository.AddAsync(entity);
       await UnitOfWork.CommitAsync();
       return ApiResponse<CompanyResponse>.Success(HttpStatusCode.Created, Mapper.Map<CompanyResponse>(entity),
           "Company Created Successfully");
    }

    public async Task<ApiResponse> UpdateAsync(UpdateCompanyRequest request)
    {
        var existingCompany =await Repository.GetByIdAsync(request.Id);
        if (existingCompany is null)    
        {
            return ApiResponse.Fail(HttpStatusCode.NotFound, "Company Not Found");
        }
        
        Mapper.Map(request, existingCompany);
        
        Repository.Update(existingCompany);
        await UnitOfWork.CommitAsync();
        
        return ApiResponse.Success(HttpStatusCode.NoContent, "Company Updated Successfully");
    }
}