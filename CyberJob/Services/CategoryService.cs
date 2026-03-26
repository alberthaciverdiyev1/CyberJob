using Microsoft.EntityFrameworkCore;
using CyberJob.Database;
using CyberJob.Models;
using CyberJob.Helpers;
using CyberJob.DTOs; 

namespace CyberJob.Services;

public class CategoryService(AppDbContext context)
{
    public async Task<List<CategoryDto>> GetOnlyParentsAsync(string lang = "az")
    {
        var parents = await context.Categories
            .AsNoTracking()
            .Where(c => c.ParentId == null)
            .OrderBy(c => c.Id)
            .ToListAsync();

        return parents.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name.Translate(lang),
            Icon = c.Icon
        }).ToList();
    }

    public async Task<List<CategoryDto>> GetParentsWithChildrenAsync(string lang = "az")
    {
        var categories = await context.Categories
            .Include(c => c.SubCategories)
            .AsNoTracking()
            .Where(c => c.ParentId == null)
            .OrderBy(c => c.Id)
            .ToListAsync();

        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name.Translate(lang),
            Icon = c.Icon,
            SubCategories = c.SubCategories.Select(s => new CategoryDto
            {
                Id = s.Id,
                Name = s.Name.Translate(lang),
                Icon = s.Icon,
                ParentId = s.ParentId
            }).ToList()
        }).ToList();
    }
}