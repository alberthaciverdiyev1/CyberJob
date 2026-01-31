
namespace CyberJob.Core.Repositories;

public interface IUnitOfWork
{
    Task CommitAsync();
    
    void Commit();
}