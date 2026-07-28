using CyberJob.DTOs;
using CyberJob.Services;
using CyberJob.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CyberJob.Controllers;

[Route("rank")]
public class RankController(CompanyService companyService, FaqService faqService, LanguageService languageService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(string date = "this_month", string search = "", int skip = 0, [FromQuery] string? lang = null)
    {
        lang ??= languageService.GetCurrentLanguage();
        const int pageSize = 10;

        if (Request.Headers.ContainsKey("HX-Request") && skip > 0)
        {
            var (moreCompanies, allTotal) = await companyService.RankListAsync(date, search, skip, 10);
            ViewBag.CurrentSkip = skip;
            ViewBag.CurrentDate = date;
            ViewBag.CurrentSearch = search;
            ViewBag.TotalCount = allTotal;
            ViewBag.PageSize = 10;
            ViewBag.PageSize = pageSize;
            return PartialView("_RankCompanyRows", moreCompanies);
        }

        var (companies, totalCount) = await companyService.RankListAsync(date, search);

        var model = new RankIndexVM
        {
            Faqs = await faqService.GetListAsync("rating", lang),
            Companies = companies,
            TotalCount = totalCount
        };

        if (Request.Headers.ContainsKey("HX-Request"))
        {
            return PartialView("Index", model);
        }

        return View(model);
    }
}