using CyberJob.Services;
using Microsoft.AspNetCore.Mvc;

namespace CyberJob.Controllers;

[Route("companies")]
public class CompanyController(CompanyService service) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        const int pageSize = 12;

        var (items, totalCount) = await service.GetPagedListAsync(search, page, pageSize);

        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        ViewBag.CurrentPage = page;
        ViewBag.Search = search;

        if (Request.Headers.ContainsKey("HX-Request"))
        {
            return PartialView("_CompanyCard", items);
        }

        return View(items);
    }
}