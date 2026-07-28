using CyberJob.Database;
using CyberJob.Models;
using Microsoft.EntityFrameworkCore;
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
        if (string.IsNullOrEmpty(value))
            return value;

        bool IsAbsoluteUrl(string v) => Uri.TryCreate(v, UriKind.Absolute, out var uri)
            && (uri.Scheme == "http" || uri.Scheme == "https");

        if (key.Contains("whatsapp", StringComparison.OrdinalIgnoreCase))
        {
            value = IsAbsoluteUrl(value) ? value : $"https://wa.me/{value.TrimStart('+')}";
            if (!value.StartsWith("https://wa.me/"))
                return null;
            return value;
        }

        if (key.Contains("telegram", StringComparison.OrdinalIgnoreCase))
        {
            value = IsAbsoluteUrl(value) ? value : $"https://t.me/{value.TrimStart('@')}";
            if (!value.StartsWith("https://t.me/"))
                return null;
            return value;
        }

        return IsAbsoluteUrl(value) ? value : null;
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