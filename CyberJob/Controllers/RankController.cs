using CyberJob.DTOs;
using CyberJob.Services;
using CyberJob.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CyberJob.Controllers;

[Route("rank")]
public class RankController(CompanyService companyService, FaqService faqService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(string date = "this_month", string search = "", [FromQuery] string lang = "az")
    {
        var model = new RankIndexVM
        {
            Faqs = await faqService.GetListAsync("rating", lang),
            Companies = await companyService.RankListAsync(date, search)
        };

        if (Request.Headers.ContainsKey("HX-Request"))
        {
            return PartialView("Index", model);
        }

        return View(model);
    }
}