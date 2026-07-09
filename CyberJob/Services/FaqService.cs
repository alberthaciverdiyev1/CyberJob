using CyberJob.Database;
using CyberJob.DTOs;
using CyberJob.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;

public class FaqService(AppDbContext context)
{
    public async Task<List<FaqDto>> GetListAsync(string? type = null)
    {
        var query = context.Faqs
            .Where(f => f.DeletedAt == null);

        if (!string.IsNullOrEmpty(type))
        {
            query = query.Where(f => f.Type == type);
        }

        return await query
            .AsNoTracking()
            .Select(f => new FaqDto
            {
                Question = f.Question!.Translate(),
                Answer = f.Answer!.Translate(),
                Type = f.Type
            })
            .ToListAsync();
    }
}