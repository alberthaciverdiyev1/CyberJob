using Microsoft.EntityFrameworkCore;
using CyberJob.Database;
using CyberJob.Models;
using CyberJob.Helpers;
using CyberJob.DTOs; 

namespace CyberJob.Services;

public class CategoryService(AppDbContext context)
{
// Dosya: e.g., YourProject.Application/Services/CategoryService.cs

    public async Task<List<CategoryDto>> GetOnlyParentsAsync(string lang = "az", int limit = 0)
    {
        IQueryable<Category> query = context.Categories
            .AsNoTracking()
            .Where(c => c.ParentId == null);

        if (limit > 0)
        {
            query = query.Take(limit);
        }

        return await query
            .OrderBy(c => c.Id)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name.Translate(lang), 
                Icon = c.Icon
            })
            .ToListAsync();
    }

    public async Task<List<CategoryDto>> GetParentsWithChildrenAsync(string lang = "az")
    {
        var categories = await context.Categories
            .Include(c => c.SubCategories)
            .AsNoTracking()
            .Where(c => c.ParentId == null)
            .OrderBy(c => c.Id)
            .ToListAsync();

        var vacancyCounts = await context.Vacancies
            .Where(v => v.IsActive && v.DeletedAt == null)
            .Where(v => v.CreatedAt >= DateTime.UtcNow.AddMonths(-1))
            .GroupBy(v => v.CategoryId)
            .Select(g => new { CategoryId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.CategoryId, x => x.Count);

        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name.Translate(lang),
            Icon = c.Icon,
            VacancyCount = vacancyCounts.GetValueOrDefault(c.Id, 0),
            SubCategories = c.SubCategories.Select(s => new CategoryDto
            {
                Id = s.Id,
                Name = s.Name.Translate(lang),
                Icon = s.Icon,
                ParentId = s.ParentId,
                VacancyCount = vacancyCounts.GetValueOrDefault(s.Id, 0)
            }).ToList()
        }).ToList();
    }
}