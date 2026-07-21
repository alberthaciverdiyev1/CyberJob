using CyberJob.Database;
using CyberJob.DTOs;
using CyberJob.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Services;

public class BlogService(AppDbContext context)
{
    public async Task<List<BlogListDto>> GetAll(string lang = "az", string? search = null)
    {
        var query = context.Blogs
            .AsNoTracking()
            .Where(b => b.IsActive && b.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(b =>
                (b.Title != null && EF.Functions.ILike(b.Title, $"%{searchLower}%")) ||
                (b.Description != null && EF.Functions.ILike(b.Description, $"%{searchLower}%")));
        }

        return await query
            .OrderByDescending(b => b.CreatedAt)
            .Select(b => new BlogListDto
            {
                Id = b.Id,
                Title = b.Title.Translate(lang),
                Description = b.Description.Translate(lang),
                Image = b.Image.ToAdminUrl(),
                ReadCount = b.ReadCount,
                CreatedAt = b.CreatedAt.ToRelativeDate(lang)
            })
            .ToListAsync();
    }

    public async Task<BlogDetailDto?> GetBlogById(int id, string lang = "az")
    {
        var blog = await context.Blogs
            .AsNoTracking()
            .Where(b => b.Id == id && b.IsActive && b.DeletedAt == null)
            .FirstOrDefaultAsync();

        if (blog == null) return null;

        // Increment read count
        await context.Blogs
            .Where(b => b.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(b => b.ReadCount, b => b.ReadCount + 1));

        return new BlogDetailDto
        {
            Id = blog.Id,
            Title = blog.Title.Translate(lang),
            Description = blog.Description.Translate(lang),
            Image = blog.Image.ToAdminUrl(),
            ReadCount = blog.ReadCount,
            CreatedAt = blog.CreatedAt.ToRelativeDate(lang)
        };
    }
}
