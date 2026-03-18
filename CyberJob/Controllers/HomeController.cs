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
                Take = 12
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
}