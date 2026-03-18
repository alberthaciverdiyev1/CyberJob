using Microsoft.EntityFrameworkCore;
using CyberJob.Database;
using CyberJob.Helpers;

namespace CyberJob.Services;

public class CityService(AppDbContext context)
{
    public async Task<object> GetAllAsync(string lang = "az")
    {
        var cities = await context.Cities
            .AsNoTracking()
            .Where(c => c.DeletedAt == null)
            .OrderBy(c => c.Id)
            .ToListAsync();

        return cities.Select(c => new
        {
            c.Id,
            Name = c.Name.Translate(lang) 
        }).ToList();
    }
}