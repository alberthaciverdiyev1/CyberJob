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
}