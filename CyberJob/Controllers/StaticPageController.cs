using CyberJob.DTOs;
using CyberJob.Helpers;
using CyberJob.Models;
using CyberJob.Services;
using CyberJob.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CyberJob.Controllers;

public class StaticPageController(FaqService faqService, SubscriptionPlanService subscriptionPlanService, LegalTermAndUserAgreementService legalTermService, SettingHelper settingHelper, LanguageService languageService) : Controller
{
    [HttpGet("/services")]
    public async Task<IActionResult> Services([FromQuery] string? lang = null)
    {
        lang ??= languageService.GetCurrentLanguage();
        ServicesVM model = new ServicesVM()
        {
            Faqs = await faqService.GetListAsync("service", lang),
            Plans = await subscriptionPlanService.GetActivePlansAsync(lang)
        };
        return View(model);
    }

    [HttpGet("/about")]
    public async Task<IActionResult> About([FromQuery] string? lang = null)
    {
        lang ??= languageService.GetCurrentLanguage();
        var rawJson = await settingHelper.Get("about_us");
        ViewBag.AboutUs = rawJson?.Translate(lang) ?? "";
        return View();
    }

    [HttpGet("/advertise")]
    public IActionResult Advertise()
    {
        return View();
    }

    [HttpGet("/contact")]
    public IActionResult Contact()
    {
        return View();
    }

    [HttpGet("/privacy")]
    public async Task<IActionResult> Privacy([FromQuery] string? lang = null)
    {
        lang ??= languageService.GetCurrentLanguage();
        var data = await legalTermService.GetAllActiveAsync(lang: lang, type:"privacy");
        return View(data);
    }

    [HttpGet("/user-agreement")]
    public async Task<IActionResult> UserAgreement([FromQuery] string? lang = null)
    {
        lang ??= languageService.GetCurrentLanguage();
        var data = await legalTermService.GetAllActiveAsync(lang: lang, type:"terms");
        return View(data);
    }
}