using CyberJob.Services;
using Microsoft.AspNetCore.Mvc;

namespace CyberJob.Controllers;
[Route("blogs")] 
public class BlogController(BlogService blogService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index() 
    {
        var blogs = await blogService.GetAll(); 
    
        return View(blogs);
    }
    [HttpGet("{id:int}")]

    public async Task<IActionResult> Details(int id)
    {
        var blog =await blogService.GetBlogById(id);
        return View(blog);
    }
}