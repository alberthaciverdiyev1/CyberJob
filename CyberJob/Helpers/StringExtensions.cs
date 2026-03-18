using System.Text.Json;

namespace CyberJob.Helpers;

public static class StringExtensions
{
    public static string Translate(this string? jsonString, string lang = "az")
    {
        if (string.IsNullOrWhiteSpace(jsonString)) 
            return string.Empty;

        try
        {
            var translations = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
            if (translations != null && translations.TryGetValue(lang, out var value))
            {
                return value;
            }
            return translations?.Values.FirstOrDefault() ?? string.Empty;
        }
        catch
        {
            return jsonString;
        }
    }
}