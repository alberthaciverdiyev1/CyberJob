namespace CyberJob.Core.Entities;

public class Setting : BaseEntity
{
    public string? Whatsapp { get; set; }
    public string? Telegram { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Linkedin { get; set; }
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
    public string? WorkingHours { get; set; }
}