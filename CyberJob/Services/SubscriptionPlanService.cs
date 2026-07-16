using CyberJob.Database;
using CyberJob.DTOs;
using CyberJob.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;

public class SubscriptionPlanService(AppDbContext context)
{
    public async Task<List<SubscriptionPlanDto>> GetActivePlansAsync(string lang = "az")
    {
        var plans = await context.SubscriptionPlans
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
                DiscountStart = p.DiscountStart,
                DiscountEnd = p.DiscountEnd,
                Options = p.Options.Select(o => new SubscriptionPlanOptionDto
                {
                    Id = o.Id,
                    Name = o.Name.Translate(lang),
                }).ToList(),
            })
            .ToListAsync();

        // Mark plan as premium if its name contains "premium"
        foreach (var plan in plans)
        {
            if (plan.Name.Contains("premium", StringComparison.OrdinalIgnoreCase))
                plan.IsPremium = true;
        }

        return plans;
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
                DiscountStart = p.DiscountStart,
                DiscountEnd = p.DiscountEnd,
                Options = p.Options.Select(o => new SubscriptionPlanOptionDto
                {
                    Id = o.Id,
                    Name = o.Name.Translate(lang),
                }).ToList(),
            })
            .ToListAsync();
    }
}
