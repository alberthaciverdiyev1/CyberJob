using CyberJob.Database;
using CyberJob.DTOs;
using CyberJob.Helpers;
using CyberJob.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;

public class BannerService(AppDbContext context)
{
    public async Task<List<BannerDto>> GetListAsync() 
    {
        return await context.Banners
            .AsNoTracking()
            .Where(b => b.IsActive && b.DeletedAt == null && b.ExpirationDate >= DateTime.UtcNow && b.StartAt <= DateTime.UtcNow)
            .OrderByDescending(b => b.ExpirationDate)
            .Select(b => new BannerDto
            {
                Id = b.Id,
                Image = b.Image.ToAdminUrl(),
                Location = b.Location,
                Link = b.Link,
                IsDesktop = b.IsDesktop
            })
            .ToListAsync(); 
    }
}