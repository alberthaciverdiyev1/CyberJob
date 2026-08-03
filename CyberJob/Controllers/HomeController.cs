using CyberJob.Models;
using Microsoft.AspNetCore.Mvc;
using CyberJob.Services;
using Microsoft.AspNetCore.RateLimiting;

namespace CyberJob.Controllers;

public class HomeController(
    BannerService bannerService,
    VacancyService vacancyService,
    CategoryService categoryService,
    PartnerService partnerService,
    StatisticsService statisticsService,
    SubscribeService subscribeService,
    LanguageService languageService) : Controller
{
    public async Task<IActionResult> Index([FromQuery] string? lang = null)
    {
        lang ??= languageService.GetCurrentLanguage();
        var banners = await bannerService.GetListAsync();
        var categories = await categoryService.GetOnlyParentsAsync(lang);
        var partners = await partnerService.GetListAsync();
        var premiumVacancies = await vacancyService.GetListAsync(new VacancyFilterParams { Lang = lang, IsPremium = true, Take = 8 });
        var latestVacancies = await vacancyService.GetListAsync(new VacancyFilterParams { Lang = lang, IsPremium = false, Take = 8 });
        var (visitorDaily, visitorWeekly, visitorMonthly, visitorTotal) = await statisticsService.GetVisitorStatsAsync();
        var (vacancyDaily, vacancyWeekly, vacancyMonthly, vacancyTotal) = await statisticsService.GetVacancyStatsAsync();

        var model = new HomeIndexVM
        {
            Banners = banners,
            HeroBanner = banners.FirstOrDefault(b => b.Location == "home_hero"),
            Categories = categories,
            Partners = partners,
            PremiumVacancies = premiumVacancies,
            LatestVacancies = latestVacancies,
            VisitorDaily = visitorDaily,
            VisitorWeekly = visitorWeekly,
            VisitorMonthly = visitorMonthly,
            VisitorTotal = visitorTotal,
            VacancyDaily = vacancyDaily,
            VacancyWeekly = vacancyWeekly,
            VacancyMonthly = vacancyMonthly,
            VacancyTotal = vacancyTotal
        };

        return View(model);
    }

    [HttpGet("/error")]
    public IActionResult Error()
    {
        return View();
    }

    [HttpGet("/not-found")]
    public IActionResult PageNotFound()
    {
        ViewBag.OriginalPath =
            HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IStatusCodeReExecuteFeature>()?.OriginalPath
            ?? HttpContext.Request.Path.Value;
        return View();
    }

    [HttpPost]
    [EnableRateLimiting("SubscribePolicy")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Subscribe([FromBody] Subscribe? request)
    {
        if (request == null)
            return BadRequest();

        var (success, message) = await subscribeService.SubscribeAsync(request.Email);
        return Json(new { success, message });
    }

}