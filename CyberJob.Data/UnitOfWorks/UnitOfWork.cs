using CyberJob.Core.Repositories;

namespace CyberJob.Data.UnitOfWorks;

public class UnitOfWork():IUnitOfWork
{
    public Task CommitAsync()
    {
        throw new NotImplementedException();
    }

    public void Commit()
    {
        throw new NotImplementedException();
    }
}