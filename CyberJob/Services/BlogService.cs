using CyberJob.Database;
using CyberJob.Models;
using CyberJob.Helpers; // Extension metodun olduğu namespace'i ekle
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;

public class BlogService(AppDbContext context)
{
    public async Task<object> GetAll(string lang = "az")
    {
        var blogsFromDb = await context.Blogs
            .AsNoTracking()
            .Where(b => b.IsActive && b.DeletedAt == null)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return blogsFromDb.Select(b => new
        {
             b.Id,
            Title = b.Title.Translate(lang),
            Description = b.Description.Translate(lang),
            Image = b.Image.ToAdminUrl(),
            b.ReadCount,
            CreatedAt = b.CreatedAt.ToRelativeDate()
        }).ToList();
    }
    public async Task<object?> GetBlogById(int id, string lang = "az")
    {
        var blog = await context.Blogs
            .AsNoTracking()
            .Where(b => b.Id == id && b.IsActive && b.DeletedAt == null)
            .FirstOrDefaultAsync();

        if (blog == null) return null;

        return new
        {
            Id = blog.Id,
            Title = blog.Title.Translate(lang),
            Description = blog.Description.Translate(lang),
            Image = blog.Image.ToAdminUrl(),
            ReadCount = blog.ReadCount,
            CreatedAt = blog.CreatedAt.ToRelativeDate() 
        };
    }
}