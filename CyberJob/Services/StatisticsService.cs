using CyberJob.Database;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;

public class StatisticsService(AppDbContext context)
{
    public async Task<(int Daily, int Weekly, int Monthly, int Total)> GetVisitorStatsAsync()
    {
        var now = DateTime.UtcNow;
        var todayStart = now.Date;
        var weekStart = todayStart.AddDays(-(int)now.DayOfWeek);
        var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        if (now.Kind != DateTimeKind.Utc)
        {
            todayStart = now.ToUniversalTime().Date;
            weekStart = todayStart.AddDays(-(int)now.ToUniversalTime().DayOfWeek);
            monthStart = new DateTime(now.ToUniversalTime().Year, now.ToUniversalTime().Month, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        var total = await context.Vacancies
            .Where(v => v.DeletedAt == null)
            .SumAsync(v => (long?)v.ViewCount) ?? 0;

        var dailyViews = await context.Vacancies
            .Where(v => v.DeletedAt == null && v.CreatedAt >= todayStart)
            .SumAsync(v => (long?)v.ViewCount) ?? 0;

        var weeklyViews = await context.Vacancies
            .Where(v => v.DeletedAt == null && v.CreatedAt >= weekStart)
            .SumAsync(v => (long?)v.ViewCount) ?? 0;

        var monthlyViews = await context.Vacancies
            .Where(v => v.DeletedAt == null && v.CreatedAt >= monthStart)
            .SumAsync(v => (long?)v.ViewCount) ?? 0;

        return ((int)dailyViews, (int)weeklyViews, (int)monthlyViews, (int)total);
    }

    public async Task<(int Daily, int Weekly, int Monthly, int Total)> GetVacancyStatsAsync()
    {
        var now = DateTime.UtcNow;
        var todayStart = now.Date;
        var weekStart = todayStart.AddDays(-(int)now.DayOfWeek);
        var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        if (now.Kind != DateTimeKind.Utc)
        {
            todayStart = now.ToUniversalTime().Date;
            weekStart = todayStart.AddDays(-(int)now.ToUniversalTime().DayOfWeek);
            monthStart = new DateTime(now.ToUniversalTime().Year, now.ToUniversalTime().Month, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        var total = await context.Vacancies
            .CountAsync(v => v.IsActive && v.DeletedAt == null);

        var daily = await context.Vacancies
            .CountAsync(v => v.IsActive && v.DeletedAt == null && v.CreatedAt >= todayStart);

        var weekly = await context.Vacancies
            .CountAsync(v => v.IsActive && v.DeletedAt == null && v.CreatedAt >= weekStart);

        var monthly = await context.Vacancies
            .CountAsync(v => v.IsActive && v.DeletedAt == null && v.CreatedAt >= monthStart);

        return (daily, weekly, monthly, total);
    }
}
