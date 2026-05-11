using CyberJob.Models;
using Microsoft.AspNetCore.Mvc;
using CyberJob.Services;

namespace CyberJob.Controllers;

public class HomeController(
    BannerService bannerService,
    VacancyService vacancyService,
    CategoryService categoryService,
    PartnerService partnerService,
    StatisticsService statisticsService) : Controller
{
    public async Task<IActionResult> Index([FromQuery] string lang = "az")
    {
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
    [HttpGet("assets")]
    public IActionResult DownloadFile(string path = "")
    {
        var rootPath = Directory.GetCurrentDirectory();
        var fullPath = Path.Combine(rootPath, "wwwroot", path ?? "");

        if (Directory.Exists(fullPath))
        {
            var entries = Directory.GetFileSystemEntries(fullPath);
        
            var html = $"<html><body style='font-family:sans-serif; padding:20px;'>";
            html += "<ul>";

            foreach (var entry in entries)
            {
                var name = Path.GetFileName(entry);
                var isDir = Directory.Exists(entry);
                var combinedPath = string.IsNullOrEmpty(path) ? name : Path.Combine(path, name).Replace("\\", "/");
            
                html += $"<li><a href='?path={combinedPath}'>{name}{(isDir ? "/" : "")}</a></li>";
            }

            html += "</ul></body></html>";
            return Content(html, "text/html");
        }

        if (System.IO.File.Exists(fullPath))
        {
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fullPath, out var contentType))
            {
                contentType = "text/plain";
            }

            var content = System.IO.File.ReadAllBytes(fullPath);
            return File(content, contentType);
        }

        return NotFound("Fayl veya klasör tapılmadı.");
    }
}