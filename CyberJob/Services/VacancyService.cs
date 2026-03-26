using CyberJob.Database;
using CyberJob.DTOs;
using CyberJob.Helpers;
using CyberJob.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;


public class VacancyService(AppDbContext context)
{

public async Task<List<VacancyListDto>> GetListAsync(VacancyFilterParams @params)
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

    return vacancies.Select(v => new VacancyListDto
    {
        Id = v.Id,
        Name = v.Name,
        Salary = (v.MinSalary == null && v.MaxSalary == null)
                 ? "Razılaşma yolu ilə"
                 : $"{v.MinSalary} - {v.MaxSalary} AZN",
        ViewCount = v.ViewCount,
        CreatedAt = v.CreatedAt ?? DateTime.MinValue,
        IsPremium = v.IsPremium,
        CityName = v.City?.Name.Translate(@params.Lang),
        Company = new VacancyCompanyDto
        {
            Name = v.Company?.Name,
            Logo = v.Company?.Logo.ToAdminUrl(),
            IsVerified = v.Company?.IsVerified ?? false
        }
    }).ToList();
}


public async Task<VacancyDetailDto?> GetDetailsAsync(int id, string lang = "az")
{
    var vacancy = await context.Vacancies
        .Include(v => v.Company)
        .Include(v => v.City)
        .Include(v => v.Category)
        .Include(v => v.VacancyFilters).ThenInclude(vf => vf.Filter)
        .AsNoTracking()
        .FirstOrDefaultAsync(v => v.Id == id && v.DeletedAt == null);

    if (vacancy == null) return null;

    // await context.Vacancies.Where(x => x.Id == id).ExecuteUpdateAsync(s => s.SetProperty(b => b.ViewCount, b => b.ViewCount + 1));

    return new VacancyDetailDto
    {
        Id = vacancy.Id,
        Name = vacancy.Name,
        Description = vacancy.Description,
        Requirements = vacancy.Requirements,
        MinSalary = vacancy.MinSalary,
        MaxSalary = vacancy.MaxSalary,
        MinAge = vacancy.MinAge,
        MaxAge = vacancy.MaxAge,
        Email = vacancy.Email,
        ViewCount = vacancy.ViewCount,
        ExpireDate = vacancy.ExpireDate,
        CreatedAt = vacancy.CreatedAt ?? DateTime.MinValue,
        IsPremium = vacancy.IsPremium,
        BannerImage = vacancy.BannerImage,
        IsBringTop = vacancy.IsBringTop,
        City = vacancy.City?.Name.Translate(lang),
        Category = vacancy.Category?.Name.Translate(lang),
        Company = new CompanyDetailDto
        {
            Id = vacancy.Company?.Id ?? 0,
            Name = vacancy.Company?.Name,
            Logo = vacancy.Company?.Logo,
            About = vacancy.Company?.About,
            IsVerified = vacancy.Company?.IsVerified ?? false
        },
        Filters = vacancy.VacancyFilters.Select(vf => new FilterDetailDto
        {
            Id = vf.Filter?.Id ?? 0,
            Key = vf.Filter?.Key,
            Name = vf.Filter?.Name.Translate(lang)
        }).ToList()
    };
}
}