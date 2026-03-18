namespace CyberJob.Helpers;

public static class TimeHelper
{
    public static string ToRelativeDate(this DateTime? dateTime)
    {
        if (!dateTime.HasValue) return "";

        var timeSpan = DateTime.Now - dateTime.Value;

        if (timeSpan <= TimeSpan.FromSeconds(60))
            return "İndicə";

        if (timeSpan <= TimeSpan.FromMinutes(60))
            return timeSpan.Minutes + " dəqiqə əvvəl";

        if (timeSpan <= TimeSpan.FromHours(24))
            return timeSpan.Hours + " saat əvvəl";

        if (timeSpan <= TimeSpan.FromDays(30))
            return timeSpan.Days + " gün əvvəl";

        if (timeSpan <= TimeSpan.FromDays(365))
            return (timeSpan.Days / 30) + " ay əvvəl";

        return (timeSpan.Days / 365) + " il əvvəl";
    }
}