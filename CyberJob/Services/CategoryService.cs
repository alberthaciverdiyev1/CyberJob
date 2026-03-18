using Microsoft.EntityFrameworkCore;
using CyberJob.Database;
using CyberJob.Models;
using CyberJob.Helpers;

namespace CyberJob.Services;

public class CategoryService(AppDbContext context)
{

    public async Task<object> GetOnlyParentsAsync(string lang = "az")
    {
        var parents = await context.Categories
            .AsNoTracking()
            .Where(c => c.ParentId == null) 
            .OrderBy(c => c.Id)
            .ToListAsync();

        return parents.Select(c => new
        {
            c.Id,
            Name = c.Name.Translate(lang),
            c.Icon
        }).ToList();
    }

    public async Task<object> GetParentsWithChildrenAsync(string lang = "az")
    {
        var categories = await context.Categories
            .Include(c => c.SubCategories) 
            .AsNoTracking()
            .Where(c => c.ParentId == null) 
            .OrderBy(c => c.Id)
            .ToListAsync();

        return categories.Select(c => new
        {
            c.Id,
            Name = c.Name.Translate(lang),
            c.Icon,
            Children = c.SubCategories.Select(s => new
            {
                s.Id,
                Name = s.Name.Translate(lang),
                s.Icon,
                s.ParentId
            }).ToList()
        }).ToList();
    }
}