namespace CyberJob.Helpers;

public static class TimeHelper
{
    public static string ToRelativeDate(this DateTime? dateTime, string lang = "az")
    {
        if (!dateTime.HasValue) return "";

        var timeSpan = DateTime.UtcNow - dateTime.Value;
        double seconds = timeSpan.TotalSeconds;

        if (seconds < 0) return GetString("time.just_now", lang);

        if (seconds < 60)
            return GetString("time.just_now", lang);

        if (seconds < 3600)
            return FormatString("time.minutes_ago", lang, (int)timeSpan.TotalMinutes);

        if (seconds < 86400)
            return FormatString("time.hours_ago", lang, (int)timeSpan.TotalHours);

        if (seconds < 2592000)
            return FormatString("time.days_ago", lang, (int)timeSpan.TotalDays);

        if (seconds < 31536000)
        {
            int months = (int)(timeSpan.TotalDays / 30);
            return FormatString("time.months_ago", lang, months <= 0 ? 1 : months);
        }

        int years = (int)(timeSpan.TotalDays / 365);
        return FormatString("time.years_ago", lang, years);
    }

    private static string GetString(string key, string lang)
    {
        var translations = GetTranslations(lang);
        return translations.TryGetValue(key, out var val) ? val : key;
    }

    private static string FormatString(string key, string lang, int number)
    {
        var translations = GetTranslations(lang);
        var template = translations.TryGetValue(key, out var val) ? val : key;
        return string.Format(template, number);
    }

    private static Dictionary<string, string> GetTranslations(string lang)
    {
        var az = new Dictionary<string, string>
        {
            ["time.just_now"] = "İndicə",
            ["time.minutes_ago"] = "{0} dəqiqə əvvəl",
            ["time.hours_ago"] = "{0} saat əvvəl",
            ["time.days_ago"] = "{0} gün əvvəl",
            ["time.months_ago"] = "{0} ay əvvəl",
            ["time.years_ago"] = "{0} il əvvəl",
        };
        var en = new Dictionary<string, string>
        {
            ["time.just_now"] = "Just now",
            ["time.minutes_ago"] = "{0} minutes ago",
            ["time.hours_ago"] = "{0} hours ago",
            ["time.days_ago"] = "{0} days ago",
            ["time.months_ago"] = "{0} months ago",
            ["time.years_ago"] = "{0} years ago",
        };
        var ru = new Dictionary<string, string>
        {
            ["time.just_now"] = "Только что",
            ["time.minutes_ago"] = "{0} минут назад",
            ["time.hours_ago"] = "{0} часов назад",
            ["time.days_ago"] = "{0} дней назад",
            ["time.months_ago"] = "{0} месяцев назад",
            ["time.years_ago"] = "{0} лет назад",
        };
        return lang switch
        {
            "en" => en,
            "ru" => ru,
            _ => az
        };
    }
}