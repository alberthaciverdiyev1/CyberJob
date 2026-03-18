using Microsoft.EntityFrameworkCore;
using CyberJob.Database;
using CyberJob.Models;
using CyberJob.Helpers;

namespace CyberJob.Services;


public class VacancyService(AppDbContext context)
{

    public async Task<List<dynamic>> GetListAsync(VacancyFilterParams @params)
    {
        var query = context.Vacancies
            .Include(v => v.Company)
            .Include(v => v.City)
            .AsNoTracking()
            .Where(v => v.IsActive && v.DeletedAt == null);

        if (@params.CityId.HasValue) 
            query = query.Where(v => v.CityId == @params.CityId);

        if (@params.CategoryId.HasValue) 
            query = query.Where(v => v.CategoryId == @params.CategoryId);

        if (@params.IsPremium.HasValue)
            query = query.Where(v => v.IsPremium == @params.IsPremium);

        if (!string.IsNullOrEmpty(@params.Search))
            query = query.Where(v => v.Name.ToLower().Contains(@params.Search.ToLower()));

        if (@params.FilterId.HasValue)
            query = query.Where(v => v.VacancyFilters.Any(vf => vf.FilterId == @params.FilterId));

        var vacancies = await query
            .OrderByDescending(v => v.CreatedAt)
            .Take(@params.Take)
            .ToListAsync();

        return vacancies.Select(v => (dynamic)new
        {
            v.Id,
            v.Name,
            Salary = v.MinSalary == null && v.MaxSalary == null ? "Razılaşma yolu ilə" : 
                $"{v.MinSalary} - {v.MaxSalary} AZN",
            v.ViewCount,
            v.CreatedAt,
            v.IsPremium,
            CityName = v.City?.Name.Translate(@params.Lang),
            Company = new {
                Name = v.Company?.Name,
                Logo = v.Company?.Logo,
                IsVerified = v.Company?.IsVerified ?? false
            }
        }).ToList();
    }

   
    public async Task<object?> GetDetailsAsync(int id, string lang = "az")
    {
        var vacancy = await context.Vacancies
            .Include(v => v.Company)
            .Include(v => v.City)
            .Include(v => v.Category)
            .Include(v => v.VacancyFilters).ThenInclude(vf => vf.Filter)
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id && v.DeletedAt == null);

        if (vacancy == null) return null;

        // context.Vacancies.Where(x => x.Id == id).ExecuteUpdate(s => s.SetProperty(b => b.ViewCount, b => b.ViewCount + 1));

        return new
        {
            vacancy.Id,
            vacancy.Name,
            vacancy.Description,
            vacancy.Requirements,
            vacancy.MinSalary,
            vacancy.MaxSalary,
            vacancy.MinAge,
            vacancy.MaxAge,
            vacancy.Email,
            vacancy.ViewCount,
            vacancy.ExpireDate,
            vacancy.CreatedAt,
            vacancy.IsPremium,
            vacancy.BannerImage,
            vacancy.IsBringTop,
            City = vacancy.City?.Name.Translate(lang),
            Category = vacancy.Category?.Name.Translate(lang),
            Company = new {
                vacancy.Company?.Id,
                vacancy.Company?.Name,
                vacancy.Company?.Logo,
                vacancy.Company?.About,
                vacancy.Company?.IsVerified
            },
            Filters = vacancy.VacancyFilters.Select(vf => new {
                vf.Filter?.Id,
                vf.Filter?.Key,
                Name = vf.Filter?.Name.Translate(lang)
            }).ToList()
        };
    }
}