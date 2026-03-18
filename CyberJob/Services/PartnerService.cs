using CyberJob.Database;
using CyberJob.Helpers;
using CyberJob.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;

public class PartnerService(AppDbContext context)
{
    public async Task<object> GetListAsync()
    {
        return await context.Partners
            .AsNoTracking()
            .Where(p => p.IsActive == true && p.DeletedAt == null)
            .OrderByDescending(p => p.Id)
            .Select(p => new
            {
                p.Id,
                Image = p.Image.ToAdminUrl(),
                p.Link
            })
            .ToListAsync();
    }
}