using CyberJob.DTOs;
using CyberJob.Models;
using CyberJob.Services;
using CyberJob.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CyberJob.Controllers;

public class StaticPageController(FaqService faqService, SubscriptionPlanService subscriptionPlanService, LegalTermAndUserAgreementService legalTermService) : Controller
{
    [HttpGet("/services")]
    public async Task<IActionResult> Services()
    {
        ServicesVM model = new ServicesVM()
        {
            Faqs = await faqService.GetListAsync("service"),
            Plans = await subscriptionPlanService.GetActivePlansAsync()
        };
        return View(model);
    }

    [HttpGet("/about")]
    public Task<IActionResult> About()
    {
        return Task.FromResult<IActionResult>(View());
    }

    [HttpGet("/advertise")]
    public Task<IActionResult> Advertise()
    {
        return Task.FromResult<IActionResult>(View());
    }

    [HttpGet("/contact")]
    public async Task<IActionResult> Contact()
    {
        return View();
    }
    
    [HttpGet("/privacy")]
    public async Task<IActionResult> Privacy()
    {
        var data = await legalTermService.GetAllActiveAsync(type:"privacy");
        return View(data);
    }

    [HttpGet("/user-agreement")]
    public async Task<IActionResult> UserAgreement()
    {
        var data = await legalTermService.GetAllActiveAsync(type:"terms");
        return View(data);
    }
}