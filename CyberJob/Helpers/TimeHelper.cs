namespace CyberJob.Helpers;

public static class TimeHelper
{
    public static string ToRelativeDate(this DateTime? dateTime)
    {
        if (!dateTime.HasValue) return "";

        var timeSpan = DateTime.Now - dateTime.Value;
        double seconds = timeSpan.TotalSeconds;

        if (seconds < 0) return "Buna hazırlaşırıq :)"; 

        if (seconds < 60)
            return "İndicə";

        if (seconds < 3600) // 60 dəqiqə
            return $"{(int)timeSpan.TotalMinutes} dəqiqə əvvəl";

        if (seconds < 86400) // 24 saat
            return $"{(int)timeSpan.TotalHours} saat əvvəl";

        if (seconds < 2592000) // 30 gün
            return $"{(int)timeSpan.TotalDays} gün əvvəl";

        if (seconds < 31536000) // 365 gün
        {
            int months = (int)(timeSpan.TotalDays / 30);
            return $"{(months <= 0 ? 1 : months)} ay əvvəl";
        }

        int years = (int)(timeSpan.TotalDays / 365);
        return $"{years} il əvvəl";
    }
}