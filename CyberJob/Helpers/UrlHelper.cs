namespace CyberJob.Helpers;

public static class UrlHelper
{
    private static string? _storageUrl;

    public static void Initialize(IConfiguration configuration)
    {
        _storageUrl = configuration["AppSettings:AdminStorageUrl"];
    }

    public static string ToAdminUrl(this string? path)
    {
        if (string.IsNullOrEmpty(path)) return "/images/no-image.png"; 
        if (path.StartsWith("http")) return path; 

        return $"{_storageUrl?.TrimEnd('/')}/{path.TrimStart('/')}";
    }
}