using CyberJob.Services;
using Microsoft.AspNetCore.Mvc;

namespace CyberJob.Controllers;

[Route("companies")]

public class CompanyController(CompanyService service) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        int pageSize = 12; 
        var result = await service.GetPagedListAsync(search, page, pageSize);

        ViewBag.TotalPages = (int)Math.Ceiling(result.TotalCount / (double)pageSize);
        ViewBag.CurrentPage = page;
        ViewBag.Search = search;

        return View(result.Items);
    }
}