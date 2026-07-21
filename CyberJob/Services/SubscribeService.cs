using CyberJob.Database;
using CyberJob.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CyberJob.Services;

public class SubscribeService(AppDbContext context, TranslationService translationService, LanguageService languageService)
{
    public async Task<(bool Success, string Message)> SubscribeAsync(string email)
    {
        var lang = languageService.GetCurrentLanguage();

        if (string.IsNullOrWhiteSpace(email))
            return (false, translationService.Get("subscribe.error_empty", lang));

        email = email.Trim();

        var subscribe = new Subscribe { Email = email };
        var validationContext = new ValidationContext(subscribe);
        var validationResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(subscribe, validationContext, validationResults, true))
        {
            var error = validationResults.FirstOrDefault()?.ErrorMessage;
            return (false, error ?? translationService.Get("validation.email", lang));
        }

        var exists = await context.Subscribes
            .AnyAsync(s => s.Email == email && s.DeletedAt == null);

        if (exists)
            return (false, translationService.Get("subscribe.exists", lang));

        context.Subscribes.Add(new Subscribe { Email = email });
        await context.SaveChangesAsync();

        return (true, translationService.Get("subscribe.success", lang));
    }
}
