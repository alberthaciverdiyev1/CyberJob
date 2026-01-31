using System.Linq.Expressions;
using CyberJob.Core.DTOs.Common;

namespace CyberJob.Core.Services;

public interface IGenericService<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    Task<ApiResponse<TDto>> GetByIdAsync(int id);

    Task<ApiResponse<IEnumerable<TDto>>> GetAllAsync();

    Task<ApiResponse<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> expression);
    

    Task<ApiResponse> RemoveAsync(int id);
}