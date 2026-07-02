using CyberJob.Database;
using CyberJob.DTOs;
using CyberJob.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;

public class SubscriptionPlanService(AppDbContext context)
{
    public async Task<List<SubscriptionPlanDto>> GetActivePlansAsync(string lang = "az")
    {
        return await context.SubscriptionPlans
            .AsNoTracking()
            .Include(p => p.Options.Where(o => o.IsActive))
            .Where(p => p.IsActive)
            .OrderBy(p => p.OldPrice)
            .Select(p => new SubscriptionPlanDto
            {
                Id = p.Id,
                Name = p.Name.Translate(lang),
                OldPrice = p.OldPrice,
                NewPrice = p.NewPrice,
                Type = p.Type,
                Options = p.Options.Select(o => new SubscriptionPlanOptionDto
                {
                    Id = o.Id,
                    Name = o.Name.Translate(lang),
                }).ToList(),
            })
            .ToListAsync();
    }

    public async Task<List<SubscriptionPlanDto>> GetAllPlansAsync(string lang = "az")
    {
        return await context.SubscriptionPlans
            .AsNoTracking()
            .Include(p => p.Options.Where(o => o.IsActive))
            .OrderBy(p => p.OldPrice)
            .Select(p => new SubscriptionPlanDto
            {
                Id = p.Id,
                Name = p.Name.Translate(lang),
                OldPrice = p.OldPrice,
                NewPrice = p.NewPrice,
                Type = p.Type,
                Options = p.Options.Select(o => new SubscriptionPlanOptionDto
                {
                    Id = o.Id,
                    Name = o.Name.Translate(lang),
                }).ToList(),
            })
            .ToListAsync();
    }
}
