using CyberJob.Database;
using CyberJob.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CyberJob.Services;

public class SubscribeService(AppDbContext context)
{
    public async Task<(bool Success, string Message)> SubscribeAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return (false, "Email daxil edin.");

        email = email.Trim();

        var subscribe = new Subscribe { Email = email };
        var validationContext = new ValidationContext(subscribe);
        var validationResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(subscribe, validationContext, validationResults, true))
        {
            var error = validationResults.FirstOrDefault()?.ErrorMessage;
            return (false, error ?? "Düzgün email ünvanı daxil edin.");
        }

        var exists = await context.Subscribes
            .AnyAsync(s => s.Email == email && s.DeletedAt == null);

        if (exists)
            return (false, "Bu email artıq abunədir.");

        context.Subscribes.Add(new Subscribe { Email = email });
        await context.SaveChangesAsync();

        return (true, "Abunəlik uğurlu oldu!");
    }
}
