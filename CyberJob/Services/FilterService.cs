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

        var childFilterIds = filters
            .SelectMany(f => f.SubFilters)
            .Where(s => s.DeletedAt == null)
            .Select(s => s.Id)
            .ToList();

        var vacancyCounts = new Dictionary<int, int>();
        if (childFilterIds.Count != 0)
        {
            vacancyCounts = await context.VacancyFilters
                .Where(vf => childFilterIds.Contains(vf.FilterId)
                    && vf.Vacancy.IsActive
                    && vf.Vacancy.DeletedAt == null)
                .GroupBy(vf => vf.FilterId)
                .Select(g => new { FilterId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.FilterId, x => x.Count);
        }

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
                    Name = s.Name.Translate(lang),
                    VacancyCount = vacancyCounts.GetValueOrDefault(s.Id, 0)
                })
                .OrderBy(s => s.Id)
                .ToList()
        }).ToList();
    }
}