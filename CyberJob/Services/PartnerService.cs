using CyberJob.Database;
using CyberJob.DTOs;
using CyberJob.Helpers;
using CyberJob.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;

public class PartnerService(AppDbContext context)
{
    public async Task<List<PartnerDto>> GetListAsync()
    {
        return await context.Partners
            .AsNoTracking()
            .Where(p => p.IsActive == true && p.DeletedAt == null)
            .OrderByDescending(p => p.Id)
            .Select(p => new PartnerDto
            {
                Id = p.Id,
                Image = p.Image.ToAdminUrl(),
                Link = p.Link
            })
            .ToListAsync();
    }
}