using CyberJob.Database;
using CyberJob.DTOs;
using CyberJob.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;

public class LegalTermAndUserAgreementService(AppDbContext context)
{
    public async Task<LegalTermAndUserAgreementDto?> GetActiveByTypeAsync(string type, string lang = "az")
    {
        return await context.LegalTermAndUserAgreements
            .Where(l => l.Type == type && l.IsActive)
            .AsNoTracking()
            .Select(l => new LegalTermAndUserAgreementDto
            {
                Title = l.Title.Translate(lang),
                Content = l.Content.Translate(lang),
                Type = l.Type,
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<LegalTermAndUserAgreementDto>> GetAllActiveAsync(string lang = "az",string type = "")
    {
        return await context.LegalTermAndUserAgreements
            .Where(l => l.IsActive)
            .Where(l => type == "" || l.Type == type)
            .AsNoTracking()
            .Select(l => new LegalTermAndUserAgreementDto
            {
                Title = l.Title.Translate(lang),
                Content = l.Content.Translate(lang),
                Type = l.Type,
            })
            .ToListAsync();
    }
}
