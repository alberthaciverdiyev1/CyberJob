using CyberJob.Database;
using CyberJob.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;

public class CompanyService(AppDbContext context)
{

    public async Task<(IEnumerable<object> Items, int TotalCount)> GetPagedListAsync(string? search, int page, int pageSize)
    {
        var query = context.Companies
            .AsNoTracking()
            .Where(c => c.IsActive && c.DeletedAt == null);

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c => c.Name.Contains(search));
        }

        int totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new
            {
                c.Id,
                c.Name,
                Logo = c.Logo.ToAdminUrl(), 
                c.IsVerified,
                c.IsActive,
                VacancyCount = c.Vacancies.Count 
            })
            .ToListAsync();

        return (items, totalCount);
    }
    
    public async Task<object?> GetDetailsAsync(int id, string lang = "az")
    {
        var company = await context.Companies
            .Include(c => c.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (company == null) return null;

        return new
        {
            company.Id,
            company.Name,
            company.Email,
            company.Phone,
            company.Address,
            About = company.About.Translate(lang),
            company.Logo,
            company.CoverImage,
            company.BannerImage,
            company.IsVerified,
            company.CreatedAt,
            company.CompanyCategoryId,
            CategoryName = company.Category?.Name.Translate(lang) ?? "" 
        };
    }
}