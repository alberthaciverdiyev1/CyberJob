using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using CyberJob.Core.DTOs.Common;
using CyberJob.Core.Repositories;
using CyberJob.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Service.Services;

public class GenericService<TEntity, TDto>(
    IGenericRepository<TEntity> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IGenericService<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    protected readonly IGenericRepository<TEntity> Repository = repository;
    protected readonly IMapper Mapper = mapper;
    protected readonly IUnitOfWork UnitOfWork = unitOfWork;

    public async Task<ApiResponse<TDto>> GetByIdAsync(int id)
    {
        var entity = await Repository.GetByIdAsync(id);
        
        if (entity == null)
            return ApiResponse<TDto>.Fail(HttpStatusCode.NotFound, $"{typeof(TEntity).Name} not found.");

        var data = Mapper.Map<TDto>(entity);
        return ApiResponse<TDto>.Success(HttpStatusCode.OK, data);
    }

    public async Task<ApiResponse<IEnumerable<TDto>>> GetAllAsync()
    {
        var entities = await Repository.GetAll().ToListAsync();
        var dtos = Mapper.Map<IEnumerable<TDto>>(entities);
        
        return ApiResponse<IEnumerable<TDto>>.Success(HttpStatusCode.OK, dtos);
    }

    public async Task<ApiResponse<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> expression)
    {
        var entities = await Repository.Where(expression).ToListAsync();
        var dtos = Mapper.Map<IEnumerable<TDto>>(entities);
        
        return ApiResponse<IEnumerable<TDto>>.Success(HttpStatusCode.OK, dtos);
    }

    public async Task<ApiResponse> RemoveAsync(int id)
    {
        var entity = await Repository.GetByIdAsync(id);
        
        if (entity == null)
            return ApiResponse.Fail(HttpStatusCode.NotFound, $"{typeof(TEntity).Name} not found.");

        Repository.Remove(entity);
        await UnitOfWork.CommitAsync();
        
        return ApiResponse.Success(HttpStatusCode.OK, $"{typeof(TEntity).Name} successfully removed.");
    }
}