using System.Text.Json;

namespace CyberJob.Services;

public class TranslationService
{
    private readonly Dictionary<string, Dictionary<string, string>> _translations = new();
    private readonly string _resourcesPath;

    public TranslationService(IWebHostEnvironment env)
    {
        _resourcesPath = Path.Combine(env.ContentRootPath, "Resources");
        LoadResources();
    }

    private void LoadResources()
    {
        var files = new[] { ("ui.az.json", "az"), ("ui.en.json", "en"), ("ui.ru.json", "ru") };
        foreach (var (fileName, lang) in files)
        {
            var path = Path.Combine(_resourcesPath, fileName);
            if (!File.Exists(path)) continue;

            try
            {
                var json = File.ReadAllText(path);
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                if (dict != null)
                    _translations[lang] = dict;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading translation resource '{fileName}': {ex.Message}");
                _translations[lang] = new Dictionary<string, string>();
            }
        }
    }

    public string Get(string key, string lang)
    {
        if (!LanguageService.IsValid(lang)) lang = "az";

        if (_translations.TryGetValue(lang, out var langDict) &&
            langDict.TryGetValue(key, out var value))
            return value;

        // Fallback to Azerbaijani
        if (_translations.TryGetValue("az", out var azDict) &&
            azDict.TryGetValue(key, out var azValue))
            return azValue;

        // Fallback to key itself (for debugging)
        return key;
    }

    public bool Exists(string key, string lang)
    {
        return _translations.TryGetValue(lang, out var dict) && dict.ContainsKey(key);
    }
}
