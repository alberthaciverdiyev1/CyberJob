using CyberJob.Services;
using Microsoft.AspNetCore.Mvc;

namespace CyberJob.Controllers;
[Route("blogs")] 
public class BlogController(BlogService blogService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(string? search, [FromQuery] string lang = "az")
    {
        var blogs = await blogService.GetAll(lang: lang, search: search);

        return View(blogs);
    }
    [HttpGet("{id:int}")]

    public async Task<IActionResult> Details(int id, [FromQuery] string lang = "az")
    {
        var blog = await blogService.GetBlogById(id, lang);
        if (blog == null) return NotFound();
        return View(blog);
    }
}