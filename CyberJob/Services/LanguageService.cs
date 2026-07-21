namespace CyberJob.Services;

public class LanguageService(IHttpContextAccessor httpContextAccessor)
{
    private const string CookieKey = "lang";

    public string GetCurrentLanguage()
    {
        var context = httpContextAccessor.HttpContext;
        if (context == null) return "az";

        // 1. Query string
        var queryLang = context.Request.Query["lang"].FirstOrDefault();
        if (IsValid(queryLang)) return queryLang!;

        // 2. Cookie
        var cookieLang = context.Request.Cookies[CookieKey];
        if (IsValid(cookieLang)) return cookieLang;

        return "az";
    }

    public void SetLanguage(string lang)
    {
        var context = httpContextAccessor.HttpContext;
        if (context == null || !IsValid(lang)) return;

        context.Response.Cookies.Append(CookieKey, lang, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            IsEssential = true,
            Secure = true
        });
    }

    public static bool IsValid(string? lang) =>
        lang is "az" or "en" or "ru";
}
