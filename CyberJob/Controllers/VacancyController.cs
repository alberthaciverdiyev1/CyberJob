using CyberJob.Models;
using CyberJob.Services;
using CyberJob.ViewModels; // ViewModel namespace'ini ekledik
using Microsoft.AspNetCore.Mvc;

namespace CyberJob.Controllers;

[Route("vacancies")]
public class VacancyController(
    VacancyService vacancyService,
    BannerService bannerService,
    FilterService filterService,
    CategoryService categoryService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(VacancyFilterParams @params)
    {
        var vacancies = await vacancyService.GetListAsync(@params);

        if (Request.Headers.ContainsKey("HX-Request"))
        {
            return PartialView("_VacancyListCard", vacancies);
        }

        var model = new VacancyIndexVM
        {
            Vacancies = vacancies,
            Banners = await bannerService.GetListAsync(),
            Filters = await filterService.GetFilterGroupAsync(@params.Lang),
            Categories = await categoryService.GetParentsWithChildrenAsync(@params.Lang)
        };

        return View(model);
    }
    [HttpGet("{Id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        // 1. Öncə əsas vakansiyanı gətiririk
        var vacancy = await vacancyService.GetByIdAsync(id);

        // 2. Əgər vakansiya tapılmazsa, dərhal 404 qaytarırıq
        if (vacancy == null)
        {
            return NotFound();
        }


        var similarVacancies = await vacancyService.GetListAsync(new()
        {
            CategoryId = vacancy.CategoryId
        });

        VacancyDetailsVM model = new()
        {
            Vacancy = vacancy,
            SimilarVacancies = similarVacancies
        };

        return View(model);
    }
}