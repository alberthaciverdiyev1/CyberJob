namespace CyberJob.Helpers;

public class Localizer
{
    private readonly Services.TranslationService _translationService;
    private readonly Services.LanguageService _languageService;

    public Localizer(Services.TranslationService translationService, Services.LanguageService languageService)
    {
        _translationService = translationService;
        _languageService = languageService;
    }

    public string this[string key]
    {
        get
        {
            var lang = _languageService.GetCurrentLanguage();
            return _translationService.Get(key, lang);
        }
    }

    public string Get(string key, params object[] args)
    {
        var lang = _languageService.GetCurrentLanguage();
        var value = _translationService.Get(key, lang);
        return args.Length > 0 ? string.Format(value, args) : value;
    }
}
