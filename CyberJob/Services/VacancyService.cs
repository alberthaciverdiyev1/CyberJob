using CyberJob.Database;
using CyberJob.DTOs;
using CyberJob.Helpers;
using CyberJob.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;


public class VacancyService(AppDbContext context, TranslationService translationService)
{
private static string FormatSalary(float? min, float? max, string negotiable) =>
    (min, max) switch
    {
        (null, null) => negotiable,
        (not null, null) => $"{min:0} AZN",
        (null, not null) => $"{max:0} AZN",
        (not null, not null) => $"{min:0} - {max:0} AZN"
    };

public async Task<List<VacancyListDto>> GetListAsync(VacancyFilterParams @params)
{
    var query = BuildBaseQuery(@params);

    if (@params.SortBy == "today")
        query = query.Where(v => v.CreatedAt.HasValue && v.CreatedAt.Value.Date == DateTime.UtcNow.Date);

    query = @params.SortBy switch
    {
        "salary_asc" => query.OrderByDescending(v => v.IsBringTop)
                             .ThenByDescending(v => v.IsPremium)
                             .ThenBy(v => v.MinSalary ?? 0),
        "salary_desc" => query.OrderByDescending(v => v.IsBringTop)
                              .ThenByDescending(v => v.IsPremium)
                              .ThenByDescending(v => v.MinSalary ?? 0),
        "oldest" => query.OrderByDescending(v => v.IsBringTop)
                         .ThenByDescending(v => v.IsPremium)
                         .ThenBy(v => v.CreatedAt),
        "newest" or "today" => query.OrderByDescending(v => v.IsBringTop)
                                    .ThenByDescending(v => v.IsPremium)
                                    .ThenByDescending(v => v.CreatedAt),
        "expire_date" => query.OrderByDescending(v => v.IsBringTop)
                              .ThenByDescending(v => v.IsPremium)
                              .ThenBy(v => v.ExpireDate),
        "views_asc" => query.OrderByDescending(v => v.IsBringTop)
                            .ThenByDescending(v => v.IsPremium)
                            .ThenBy(v => v.ViewCount),
        "views_desc" => query.OrderByDescending(v => v.IsBringTop)
                             .ThenByDescending(v => v.IsPremium)
                             .ThenByDescending(v => v.ViewCount),
        _ => query.OrderByDescending(v => v.IsBringTop)
                  .ThenByDescending(v => v.IsPremium)
                  .ThenByDescending(v => v.CreatedAt)
    };

    var vacancies = await query
        .Include(v => v.Company)
        .Include(v => v.City)
        .Include(v => v.VacancyFilters)
        .Take(@params.Take)
        .ToListAsync();

    return vacancies.Select(v => new VacancyListDto
    {
        Id = v.Id,
        Name = v.Name.Translate(@params.Lang),
        Salary = FormatSalary(v.MinSalary, v.MaxSalary, translationService.Get("vacancy.salary.negotiable", @params.Lang)),
        ViewCount = v.ViewCount,
        CreatedAt = v.CreatedAt,
        IsPremium = v.IsPremium,
        CityName = v.City?.Name.Translate(@params.Lang),
        Company = new VacancyCompanyDto
        {
            Name = v.Company?.Name ?? "",
            Logo = v.Company?.Logo?.ToAdminUrl() ?? "/images/no-image.png",
            IsVerified = v.Company?.IsVerified ?? false
        }
    }).ToList();
}

public async Task<int> GetFilteredCountAsync(VacancyFilterParams @params)
{
    var query = BuildBaseQuery(@params);

    if (@params.SortBy == "today")
        query = query.Where(v => v.CreatedAt.HasValue && v.CreatedAt.Value.Date == DateTime.UtcNow.Date);

    return await query.CountAsync();
}

private IQueryable<Vacancy> BuildBaseQuery(VacancyFilterParams @params)
{
    var query = context.Vacancies
        .AsNoTracking()
        .Where(v => v.IsActive && v.DeletedAt == null);

    if (@params.CityId.HasValue)
        query = query.Where(v => v.CityId == @params.CityId);
    if (@params.CategoryId.HasValue)
        query = query.Where(v => v.CategoryId == @params.CategoryId);
    if (@params.IsPremium.HasValue)
        query = query.Where(v => v.IsPremium == @params.IsPremium);
    if (@params.CompanyId.HasValue)
        query = query.Where(v => v.CompanyId == @params.CompanyId);
    if (!string.IsNullOrEmpty(@params.Search))
        query = query.Where(v => EF.Functions.ILike(v.Name, $"%{@params.Search}%"));

    if (@params.Filters != null && @params.Filters.Any())
    {
        foreach (var filter in @params.Filters)
        {
            if (!string.IsNullOrEmpty(filter.Value) && int.TryParse(filter.Key, out var filterId))
            {
                query = query.Where(v => v.VacancyFilters.Any(vf =>
                    vf.FilterId == filterId));
            }
        }
    }

    if (@params.MinSalary.HasValue)
        query = query.Where(v => v.MaxSalary >= @params.MinSalary || v.MaxSalary == null);
    if (@params.MaxSalary.HasValue)
        query = query.Where(v => v.MinSalary <= @params.MaxSalary || v.MinSalary == null);

    query = query.Where(v => v.ExpireDate >= DateTime.UtcNow);

    return query;
}

    public async Task<int> GetExpiredCountAsync()
    {
        return await context.Vacancies
            .AsNoTracking()
            .CountAsync(v => v.IsActive && v.DeletedAt == null && v.ExpireDate < DateTime.UtcNow);
    }

    public async Task<VacancyDetailDto?> GetByIdAsync(int id, string lang = "az")
    {
        var vacancy = await context.Vacancies
            .Include(v => v.Company)
            .Include(v => v.City)
            .Include(v => v.Category)
            .Include(v => v.VacancyFilters)
            .ThenInclude(vf => vf.Filter!)
                .ThenInclude(f => f.Parent)
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id && v.DeletedAt == null && v.IsActive && v.ExpireDate >= DateTime.UtcNow);

        if (vacancy == null) return null;

        await context.Vacancies
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(b => b.ViewCount, b => b.ViewCount + 1));

        return new VacancyDetailDto
        {
            Id = vacancy.Id,
            Name = vacancy.Name.Translate(lang),
            Description = vacancy.Description.Translate(lang),
            Requirements = vacancy.Requirements.Translate(lang),
            Salary = FormatSalary(vacancy.MinSalary, vacancy.MaxSalary, translationService.Get("vacancy.salary.negotiable", lang)),
            MinAge = vacancy.MinAge,
            MaxAge = vacancy.MaxAge,
            Email = vacancy.Email,
            ApplicationUrl = vacancy.ApplicationUrl,
            ViewCount = vacancy.ViewCount,
            ExpireDate = vacancy.ExpireDate,
            CreatedAt = vacancy.CreatedAt ?? DateTime.MinValue,
            IsPremium = vacancy.IsPremium,
            BannerImage = !string.IsNullOrEmpty(vacancy.BannerImage)
                ? vacancy.BannerImage.ToAdminUrlWithNoImage()
                : null,
            IsBringTop = vacancy.IsBringTop,
            City = vacancy.City?.Name.Translate(lang),
            Category = vacancy.Category?.Name.Translate(lang),
            CategoryId = vacancy.CategoryId,
            Company = new CompanyDetailDto
            {
                Id = vacancy.Company?.Id ?? 0,
                Name = vacancy.Company?.Name,
                Email = vacancy.Company?.Email,
                Logo = vacancy.Company?.Logo.ToAdminUrlWithNoImage(),
                BannerImage = vacancy.Company?.BannerImage.ToAdminUrlWithNoImage() ?? null,
                About = vacancy.Company?.About,
                IsVerified = vacancy.Company?.IsVerified ?? false
            },
            Filters = vacancy.VacancyFilters.Select(vf => new FilterDetailDto
            {
                Id = vf.Filter?.Id ?? 0,
                Key = vf.Filter?.Key,
                Name = vf.Filter?.Name.Translate(lang),
                ParentName = vf.Filter?.Parent?.Name.Translate(lang)
            }).ToList()
        };
    }
}
