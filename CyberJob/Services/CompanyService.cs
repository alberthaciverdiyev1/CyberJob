using CyberJob.Database;
using CyberJob.DTOs;
using CyberJob.Helpers;
using CyberJob.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;

public class CompanyService(AppDbContext context, TranslationService translationService)
{

    private static string FormatSalary(float? min, float? max, string negotiable) =>
        (min, max) switch
        {
            (null, null) => negotiable,
            (not null, null) => $"{min:0} AZN",
            (null, not null) => $"{max:0} AZN",
            (not null, not null) => $"{min:0} - {max:0} AZN"
        };

    public async Task<(IEnumerable<dynamic> Items, int TotalCount)> GetPagedListAsync(string? search, int? page, int? pageSize)
    {
        var query = context.Companies
            .AsNoTracking()
            .Where(c => c.IsActive && c.DeletedAt == null);

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c => EF.Functions.ILike(c.Name, $"%{search}%"));
        }

        int totalCount = await query.CountAsync();

        var orderedQuery = query.OrderByDescending(c => c.Id);

        IQueryable<Company> pagedQuery = orderedQuery;
        if (page.HasValue && pageSize.HasValue)
        {
            pagedQuery = orderedQuery
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }

        var items = await pagedQuery.Select(c => new
        {
            c.Id,
            c.Name,
            Logo = c.Logo.ToAdminUrl(),
            IsVerified = c.VerifiedAt != null,
            c.IsActive,
            VacancyCount = c.Vacancies.Count
        })
        .ToListAsync();

        return (Items: items, TotalCount: totalCount);
    }
    public async Task<CompanyDetailsDto?> GetDetailsAsync(int id, string lang = "az")
    {
        var company = await context.Companies
            .Include(c => c.Category)
            .Include(c => c.Vacancies)
            .ThenInclude(v => v.City)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (company == null) return null;

        return new CompanyDetailsDto
        {
            Id = company.Id,
            Name = company.Name,
            Email = company.Email,
            Phone = company.Phone,
            Address = company.Address,
            ShortAddress = company.ShortAddress,
            FoundedYear = company.FoundedYear,
            About = company.About.Translate(lang),
            Logo = company.Logo.ToAdminUrl(),
            CoverImage = company.CoverImage.ToAdminUrl(),
            BannerImage = company.BannerImage.ToAdminUrl(),
            IsVerified = company.VerifiedAt != null,
            CreatedAt = company.CreatedAt,
            CompanyCategoryId = company.CompanyCategoryId,
            CategoryName = company.Category?.Name.Translate(lang) ?? "",
            VacancyCount = company.Vacancies.Count,
            Vacancies = company.Vacancies
                .Where(v => v.DeletedAt == null)
                .Select(v => new VacancyListDto
                {
                    Id = v.Id,
                    Name = v.Name.Translate(lang),
                    Salary = FormatSalary(v.MinSalary, v.MaxSalary, translationService.Get("vacancy.salary.negotiable", lang)),
                    ViewCount = v.ViewCount,
                    CityName = v.City?.Name.Translate(lang),
                    CreatedAt = v.CreatedAt
                })
                .OrderByDescending(v => v.CreatedAt)
                .ToList()

        };
    }
    public async Task<(List<CompanyListDto> Companies, int TotalCount)> RankListAsync(string date, string? search, int skip = 0, int take = int.MaxValue)
    {
        var now = DateTime.UtcNow;
        DateTime? startDate = null;
        DateTime? endDate = null;

        switch (date?.ToLower())
        {
            case "this_month":
                startDate = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                break;
            case "last_month":
                var lastMonth = now.AddMonths(-1);
                startDate = new DateTime(lastMonth.Year, lastMonth.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                endDate = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddTicks(-1);
                break;
            case "all_time":
            default:
                startDate = null;
                break;
        }

        var query = context.Companies
            .AsNoTracking()
            .Where(c => c.IsActive && c.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim().ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(search));
        }

        var totalCount = await query.CountAsync();

        var companies = await query.Select(c => new CompanyListDto
        {
            Id = c.Id,
            Name = c.Name,
            Logo = c.Logo.ToAdminUrl(),
            IsVerified = c.VerifiedAt != null,
            VacancyCount = c.Vacancies.Count(v =>
                v.DeletedAt == null &&
                (!startDate.HasValue || v.CreatedAt >= startDate) &&
                (!endDate.HasValue || v.CreatedAt <= endDate))
        })
            .OrderByDescending(c => c.VacancyCount)
            .ThenBy(c => c.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return (companies, totalCount);
    }
}