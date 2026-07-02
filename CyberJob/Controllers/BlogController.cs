using CyberJob.Services;
using Microsoft.AspNetCore.Mvc;

namespace CyberJob.Controllers;
[Route("blogs")] 
public class BlogController(BlogService blogService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(string? search)
    {
        var blogs = await blogService.GetAll(search: search);

        return View(blogs);
    }
    [HttpGet("{id:int}")]

    public async Task<IActionResult> Details(int id)
    {
        var blog = await blogService.GetBlogById(id);
        return View(blog);
    }
}