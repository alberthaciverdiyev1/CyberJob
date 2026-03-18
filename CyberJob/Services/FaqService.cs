using CyberJob.Database;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;

public class FaqService(AppDbContext context)
{
    public async Task<object> GetListAsync()
    {
        return await context.Faqs
            .Where(f => f.DeletedAt == null)
            .AsNoTracking()
            .ToListAsync();
    }
}