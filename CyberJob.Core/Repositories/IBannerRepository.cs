using CyberJob.Core.Entities;

namespace CyberJob.Core.Repositories;

public interface IBannerRepository:IGenericRepository<Banner>
{
    IQueryable<Banner> GetBannersByPage(string type);
}