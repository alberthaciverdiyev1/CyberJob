using CyberJob.Core.DTOs.Common;
using CyberJob.Core.DTOs.Vacancy;
using CyberJob.Core.Entities;

namespace CyberJob.Core.Services;

public interface IVacancyService : IGenericService<Vacancy, VacancyResponse>
{
    new Task<ApiResponse<VacancyDetailsResponse>> GetVacancyDetailsByIdAsync(int id);
    Task<ApiResponse> AddAsync(CreateVacancyRequest request);
    Task<ApiResponse> UpdateAsync(UpdateVacancyRequest request);
}