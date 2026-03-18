using CyberJob.Database;
using CyberJob.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;

public class FilterService(AppDbContext context)
{
    public async Task<object> GetFilterGroupAsync(string lang = "az")
    {
        var filters = await context.Filters
            .Include(f => f.SubFilters)
            .AsNoTracking()
            .Where(f => f.ParentId == null && f.DeletedAt == null)
            .OrderBy(f => f.Id)
            .ToListAsync();

        return filters.Select(f => new
        {
            f.Id,
            f.Key, 
            Name = f.Name.Translate(lang),
            Options = f.SubFilters
                .Where(s => s.DeletedAt == null)
                .Select(s => new
                {
                    s.Id,
                    s.Key,
                    Name = s.Name.Translate(lang)
                }).ToList()
        }).ToList();
    }
}