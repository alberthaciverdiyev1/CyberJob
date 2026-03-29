using CyberJob.DTOs;
using CyberJob.Models;
using CyberJob.Services;
using CyberJob.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CyberJob.Controllers;

public class StaticPageController(FaqService faqService) : Controller
{
    [HttpGet("/services")]
    public async Task<IActionResult> Services()
    {
        ServicesVM model = new ServicesVM()
        {
            Faqs = await faqService.GetListAsync()
        };
        return View(model);
    }
}