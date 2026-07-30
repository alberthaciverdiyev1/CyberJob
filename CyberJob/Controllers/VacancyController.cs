using CyberJob.Models;
using CyberJob.Services;
using CyberJob.ViewModels; 
using Microsoft.AspNetCore.Mvc;

namespace CyberJob.Controllers;

[Route("vacancies")]
public class VacancyController(
    VacancyService vacancyService,
    BannerService bannerService,
    FilterService filterService,
    CategoryService categoryService,
    LanguageService languageService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(VacancyFilterParams @params)
    {
        @params.Lang = languageService.GetCurrentLanguage();
        var vacancies = await vacancyService.GetListAsync(@params);
        var totalCount = await vacancyService.GetFilteredCountAsync(@params);

        if (Request.Headers.ContainsKey("HX-Request"))
        {
            var hxBanners = await bannerService.GetListAsync();
            ViewBag.Banners = hxBanners;
            return PartialView("_VacancyListCard", vacancies);
        }

        var allBanners = await bannerService.GetListAsync();
        ViewBag.Banners = allBanners;

        var model = new VacancyIndexVM
        {
            Vacancies = vacancies,
            TotalCount = totalCount,
            ExpiredCount = await vacancyService.GetExpiredCountAsync(),
            Banners = allBanners,
            Filters = await filterService.GetFilterGroupAsync(@params.Lang),
            Categories = await categoryService.GetParentsWithChildrenAsync(@params.Lang)
        };

        return View(model);
    }
    [HttpGet("{Id:int}")]
    public async Task<IActionResult> Details(int id, [FromQuery] string? lang = null)
    {
        lang ??= languageService.GetCurrentLanguage();
        var vacancy = await vacancyService.GetByIdAsync(id, lang);

        if (vacancy == null)
        {
            return NotFound();
        }

        var similarVacancies = await vacancyService.GetListAsync(new()
        {
            Lang = lang,
            CategoryId = vacancy.CategoryId,
            Take = 10
        });

        // Filter out the current vacancy from similar list
        similarVacancies = similarVacancies.Where(v => v.Id != id).ToList();

        VacancyDetailsVM model = new()
        {
            Vacancy = vacancy,
            SimilarVacancies = similarVacancies
        };

        return View(model);
    }
}