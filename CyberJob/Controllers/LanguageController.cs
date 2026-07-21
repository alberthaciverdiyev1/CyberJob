using CyberJob.Services;
using Microsoft.AspNetCore.Mvc;

namespace CyberJob.Controllers;

public class LanguageController(LanguageService languageService) : Controller
{
    [HttpGet]
    public IActionResult SetLanguage(string lang, string? returnUrl)
    {
        if (!LanguageService.IsValid(lang))
            lang = "az";

        languageService.SetLanguage(lang);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return Redirect("/");
    }
}
