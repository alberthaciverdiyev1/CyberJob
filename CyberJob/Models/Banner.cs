using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberJob.Models
{
    [Table("banners")]
    public class Banner
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("image")]
        public string Image { get; set; }

        [Column("location")]
        public string Location { get; set; }

        [Column("link")]
        public string? Link { get; set; }

        [Column("start_at")]
        public DateTime StartAt { get; set; }

        [Column("expiration_date")]
        public DateTime ExpirationDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("is_desktop")]
        public bool IsDesktop { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }
    }
}