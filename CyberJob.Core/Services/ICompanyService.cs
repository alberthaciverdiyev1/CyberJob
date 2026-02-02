using CyberJob.Core.DTOs.Common;
using CyberJob.Core.DTOs.Company;
using CyberJob.Core.Entities;

namespace CyberJob.Core.Services;

public interface ICompanyService : IGenericService<Company, CompanyResponse>
{
    Task<ApiResponse<CompanyResponse>> AddAsync(CreateCompanyRequest request);
    Task<ApiResponse> UpdateAsync(UpdateCompanyRequest request);
}