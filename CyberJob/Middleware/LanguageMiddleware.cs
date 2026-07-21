using CyberJob.Services;
using System.Globalization;

namespace CyberJob.Middleware;

public class LanguageMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, LanguageService languageService)
    {
        var lang = languageService.GetCurrentLanguage();
        context.Items["lang"] = lang;

        // Set thread culture for date/number formatting
        var culture = lang switch
        {
            "en" => new CultureInfo("en-US"),
            "ru" => new CultureInfo("ru-RU"),
            _ => new CultureInfo("az-Latn-AZ")
        };
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        await next(context);
    }
}
