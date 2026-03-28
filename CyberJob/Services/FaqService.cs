using CyberJob.Database;
using CyberJob.DTOs;
using CyberJob.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;

public class FaqService(AppDbContext context)
{
    public async Task<List<FaqDto>> GetListAsync()
    {
        return await context.Faqs
            .Where(f => f.DeletedAt == null)
            .AsNoTracking()
            .Select(f => new FaqDto
            {
                Question = f.Question!.Translate(), 
                Answer = f.Answer!.Translate()
            })
            .ToListAsync();
    }
}