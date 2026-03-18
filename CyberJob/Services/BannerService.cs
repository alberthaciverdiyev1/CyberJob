using CyberJob.Database;
using CyberJob.Helpers;
using CyberJob.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;

public class BannerService(AppDbContext context)
{
    public async Task<object> GetListAsync()
    {
        return await context.Banners
            .AsNoTracking() 
            .Where(b => b.IsActive )
            .OrderByDescending(b => b.ExpirationDate)
            .Select(b => new {
                b.Id,
                Image = b.Image.ToAdminUrl(), 
                b.Location,
                b.IsDesktop
            })
            .ToListAsync();
    }
}