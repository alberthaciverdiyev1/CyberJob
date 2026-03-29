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
            return PartialView("_CompanyCard", items);
        }

        return View(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        CompanyDetailsVM model = new()
        {
            Company = await companyService.GetDetailsAsync(id),
            Cities = await cityService.GetAllAsync(),
            Filters = await filterService.GetFilterGroupAsync()
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