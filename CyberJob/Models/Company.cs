using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberJob.Models
{
    [Table("companies")]
    public class Company
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("address")]
        public string? Address { get; set; }


        [Column("short_address")]
        public string? ShortAddress { get; set; }

        [Column("founded_year")]

        public int? FoundedYear { get; set; }

        [Column("about", TypeName = "json")]
        public string? About { get; set; }

        [Column("logo")]
        public string? Logo { get; set; }

        [Column("cover_image")]
        public string? CoverImage { get; set; }

        [Column("banner_image")]
        public string? BannerImage { get; set; }

        // Foreign Key kolonu
        [Column("category_id")]
        public int CompanyCategoryId { get; set; }

        [ForeignKey("CompanyCategoryId")]
        public virtual CompanyCategory? Category { get; set; }

        [Column("is_verified")]
        public bool IsVerified { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Vacancy> Vacancies { get; set; } = new HashSet<Vacancy>();
    }
}