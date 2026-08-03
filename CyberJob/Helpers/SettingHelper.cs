using CyberJob.Database;
using CyberJob.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using QRCoder;

namespace CyberJob.Helpers;

public class SettingHelper(AppDbContext context)
{
    private Setting? _cachedSettings;
    
    public async Task<string?> Get(string key)
    {
        _cachedSettings ??= await context.Settings.FirstOrDefaultAsync();

        if (_cachedSettings == null) return null;

        var property = typeof(Setting).GetProperties()
            .FirstOrDefault(p => 
                p.Name.Equals(key, StringComparison.OrdinalIgnoreCase) || 
                p.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.ColumnAttribute>()?.Name == key);

        return property?.GetValue(_cachedSettings)?.ToString();
    }

    public async Task<string> GetScript(string type)
    {
        string columnName = type.ToLower() switch
        {
            "header" => "header_scripts",
            "body"   => "body_scripts",
            "footer" => "footer_scripts",
            _        => type
        };

        return await Get(columnName) ?? string.Empty;
    }
    
    public async Task<string?> GetSocialUrl(string key)
    {
        var value = await Get(key);
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (key.Contains("whatsapp", StringComparison.OrdinalIgnoreCase))
            return ToWaMeLink(value);

        if (key.Contains("telegram", StringComparison.OrdinalIgnoreCase))
            return ToTelegramLink(value);

        return IsAbsoluteUrl(value) ? value : null;
    }

    private static bool IsAbsoluteUrl(string value) =>
        Uri.TryCreate(value, UriKind.Absolute, out var uri)
        && (uri.Scheme == "http" || uri.Scheme == "https");

    private static string? ToWaMeLink(string value)
    {
        if (Uri.TryCreate(value, UriKind.Absolute, out var uri)
            && (uri.Scheme == "http" || uri.Scheme == "https"))
        {
            string? phone = null;
            if (uri.Host.Equals("wa.me", StringComparison.OrdinalIgnoreCase))
                phone = uri.AbsolutePath;
            else if (uri.Host.EndsWith("whatsapp.com", StringComparison.OrdinalIgnoreCase))
                phone = QueryHelpers.ParseQuery(uri.Query)["phone"].FirstOrDefault();

            if (phone is null)
                return null;
            value = phone;
        }

        var digits = new string(value.Where(char.IsDigit).ToArray());
        return digits.Length == 0 ? null : $"https://wa.me/{digits}";
    }

    private static string? ToTelegramLink(string value)
    {
        if (Uri.TryCreate(value, UriKind.Absolute, out var uri)
            && (uri.Scheme == "http" || uri.Scheme == "https")
            && uri.Host.Equals("t.me", StringComparison.OrdinalIgnoreCase))
            return $"https://t.me/{uri.AbsolutePath.Trim('/')}";

        var handle = new string(value.Trim().TrimStart('@', '+').Where(c => !char.IsWhiteSpace(c)).ToArray());
        return handle.Length == 0 ? null : $"https://t.me/{handle}";
    }

    public string GenerateQrSvg(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return string.Empty;
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new SvgQRCode(qrCodeData);

        return qrCode.GetGraphic(20);
    }
}