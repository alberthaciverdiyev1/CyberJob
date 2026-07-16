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

        bool IsUrl(string v) => v.StartsWith("http://") || v.StartsWith("https://") || v.StartsWith("wa.me") || v.StartsWith("t.me");

        if (key.Contains("whatsapp", StringComparison.OrdinalIgnoreCase))
            return IsUrl(value) ? value : $"https://wa.me/{value.TrimStart('+')}";

        if (key.Contains("telegram", StringComparison.OrdinalIgnoreCase))
            return IsUrl(value) ? value : $"https://t.me/{value.TrimStart('@')}";

        return value;
    }

    public string GenerateQrSvg(string url)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new SvgQRCode(qrCodeData);
        
        return qrCode.GetGraphic(20);
    }
}