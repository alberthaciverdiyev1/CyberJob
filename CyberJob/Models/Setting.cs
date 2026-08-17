using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberJob.Models;

[Table("settings")]
public class Setting
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

    [Column("whatsapp_number")]
    public string? WhatsappNumber { get; set; }

    [Column("whatsapp_business_number")]
    public string? WhatsappBusinessNumber { get; set; }

    [Column("instagram_url")]
    public string? InstagramUrl { get; set; }

    [Column("facebook_url")]
    public string? FacebookUrl { get; set; }

    [Column("linkedin_url")]
    public string? LinkedinUrl { get; set; }

    [Column("telegram_number")]
    public string? TelegramNumber { get; set; }

    [Column("tiktok_url")]
    public string? TiktokUrl { get; set; }

    [Column("youtube_url")]
    public string? YoutubeUrl { get; set; }

    [Column("twitter_url")]
    public string? TwitterUrl { get; set; }

    [Column("mail")]
    public string? Mail { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("working_hours")]
    public string? WorkingHours { get; set; }

    [Column("about_us")]
    public string? AboutUs { get; set; }
    
    [Column("favicon")]
    public string? Favicon { get; set; }

    [Column("header_scripts")]
    public string? HeaderScripts { get; set; }

    [Column("body_scripts")]
    public string? BodyScripts { get; set; }

    [Column("footer_scripts")]
    public string? FooterScripts { get; set; }

    // Laravel $table->timestamps() counterparts
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}
