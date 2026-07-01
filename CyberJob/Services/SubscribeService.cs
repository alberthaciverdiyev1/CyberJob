using CyberJob.Database;
using CyberJob.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;

public class SubscribeService(AppDbContext context)
{
    public async Task<(bool Success, string Message)> SubscribeAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return (false, "Email daxil edin.");

        var exists = await context.Subscribes
            .AnyAsync(s => s.Email == email && s.DeletedAt == null);

        if (exists)
            return (false, "Bu email artıq abunədir.");

        context.Subscribes.Add(new Subscribe { Email = email });
        await context.SaveChangesAsync();

        return (true, "Abunəlik uğurlu oldu!");
    }
}
