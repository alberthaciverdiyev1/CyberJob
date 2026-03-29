using Microsoft.EntityFrameworkCore;
using CyberJob.Database;
using CyberJob.Helpers;
using CyberJob.Models;

namespace CyberJob.Services;

public class CityService(AppDbContext context)
{
    public async Task<List<City>> GetAllAsync(string lang = "az")
    {
        var cities = await context.Cities
            .AsNoTracking()
            .Where(c => c.DeletedAt == null)
            .OrderBy(c => c.Id)
            .ToListAsync();

        return cities.Select(c => new City()
        {
            Id = c.Id,
            Name = c.Name.Translate(lang)
        }).ToList();
    }
}