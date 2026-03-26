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
        var banners = await bannerService.GetListAsync();
        var filters = await filterService.GetFilterGroupAsync();
        var categories = await categoryService.GetParentsWithChildrenAsync();

        var model = new VacancyIndexVM
        {
            Vacancies = vacancies,
            Banners = banners,
            Filters = filters,
            Categories = categories
        };

        if (Request.Headers.ContainsKey("HX-Request"))
        {
            return PartialView("_VacancyList", model.Vacancies);
        }

        return View(model);
    }
}