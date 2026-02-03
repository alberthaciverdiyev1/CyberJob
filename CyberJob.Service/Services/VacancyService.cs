using System.Net;
using AutoMapper;
using CyberJob.Core.DTOs.Common;
using CyberJob.Core.DTOs.Vacancy;
using CyberJob.Core.Entities;
using CyberJob.Core.Repositories;
using CyberJob.Core.Services;

namespace CyberJob.Service.Services;

public class VacancyService(IGenericRepository<Vacancy> repository, IUnitOfWork unitOfWork, IMapper mapper) 
    : GenericService<Vacancy, VacancyResponse>(repository, unitOfWork, mapper), IVacancyService
{
    public async Task<ApiResponse<VacancyDetailsResponse>> GetVacancyDetailsByIdAsync(int id)
    {
        var entity = await Repository.GetByIdAsync(id);
        
        if (entity is null)
        {
            return ApiResponse<VacancyDetailsResponse>.Fail(HttpStatusCode.NotFound, "Vacancy Not Found");
        }

        var data = Mapper.Map<VacancyDetailsResponse>(entity);
        return ApiResponse<VacancyDetailsResponse>.Success(HttpStatusCode.OK, data);
    }


    public async Task<ApiResponse> AddAsync(CreateVacancyRequest request)
    {
        var entity = Mapper.Map<Vacancy>(request);
        await Repository.AddAsync(entity);
        await UnitOfWork.CommitAsync();

        return ApiResponse.Success(HttpStatusCode.Created,"Vacancy Created Successfully");
    }

    public async Task<ApiResponse> UpdateAsync(UpdateVacancyRequest request)
    {
        var entity = Mapper.Map<Vacancy>(request);
        Repository.Update(entity);
        await UnitOfWork.CommitAsync();

        return ApiResponse.Success(HttpStatusCode.NoContent, "Vacancy Updated Successfully");
    }
}