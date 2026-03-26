using Microsoft.EntityFrameworkCore;
using CyberJob.Database;
using CyberJob.Helpers;
using CyberJob.DTOs; 

namespace CyberJob.Services;

public class FilterService(AppDbContext context)
{
    public async Task<List<FilterGroupDto>> GetFilterGroupAsync(string lang = "az")
    {
        var filters = await context.Filters
            .Include(f => f.SubFilters)
            .AsNoTracking()
            .Where(f => f.ParentId == null && f.DeletedAt == null)
            .OrderBy(f => f.Id)
            .ToListAsync();

        return filters.Select(f => new FilterGroupDto
        {
            Id = f.Id,
            Key = f.Key,
            Name = f.Name.Translate(lang),
            Options = f.SubFilters
                .Where(s => s.DeletedAt == null)
                .Select(s => new FilterOptionDto
                {
                    Id = s.Id,
                    Key = s.Key,
                    Name = s.Name.Translate(lang)
                })
                .OrderBy(s => s.Id)
                .ToList()
        }).ToList();
    }
}