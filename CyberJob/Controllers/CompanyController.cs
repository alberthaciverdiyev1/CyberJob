using CyberJob.DTOs;
using CyberJob.Models;
using CyberJob.Services;
using CyberJob.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CyberJob.Controllers;

[Route("companies")]
public class CompanyController(CompanyService companyService, CityService cityService, FilterService filterService,VacancyService vacancyService)
    : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        const int pageSize = 12;

        var (items, totalCount) = await companyService.GetPagedListAsync(search, page, pageSize);

        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        ViewBag.CurrentPage = page;
        ViewBag.Search = search;

        if (Request.Headers.ContainsKey("HX-Request"))
        {
            return PartialView("_CompanyContainer", items);
        }

        return View(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id, [FromQuery] string lang = "az")
    {
        var company = await companyService.GetDetailsAsync(id, lang);
        if (company == null) return NotFound();

        CompanyDetailsVM model = new()
        {
            Company = company,
            Cities = await cityService.GetAllAsync(lang),
            Filters = await filterService.GetFilterGroupAsync(lang)
        };
        return View(model);
    }

    [HttpGet("{id:int}/vacancies")]
    public async Task<IActionResult> Vacancies(int id, [FromQuery] VacancyFilterParams filterParams)
    {
        filterParams.CompanyId = id;
    
        var vacancies = await vacancyService.GetListAsync(filterParams);

        return PartialView("_VacancyListPartial", vacancies);
    }
}