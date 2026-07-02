using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberJob.Models;

[Table("vacancies")]
public class Vacancy
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("requirements")]
    public string Requirements { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("view_count")]
    public int ViewCount { get; set; }

    [Column("expire_date")]
    public DateTime ExpireDate { get; set; }

    [Column("banner_image")]
    public string? BannerImage { get; set; }

    [Column("min_salary")]
    public float? MinSalary { get; set; }

    [Column("max_salary")]
    public float? MaxSalary { get; set; }

    [Column("min_age")]
    public byte? MinAge { get; set; } 

    [Column("max_age")]
    public byte? MaxAge { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("is_premium")]
    public bool IsPremium { get; set; }

    [Column("is_bring_top")]
    public bool IsBringTop { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }


    [Column("city_id")]
    public int CityId { get; set; }

    [Column("company_id")]
    public int CompanyId { get; set; }

    [Column("category_id")]
    public int CategoryId { get; set; }


    [ForeignKey("CityId")]
    public virtual City? City { get; set; }

    [ForeignKey("CompanyId")]
    public virtual Company? Company { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }
    
    public virtual ICollection<VacancyFilter> VacancyFilters { get; set; } = new List<VacancyFilter>();
}

    public class VacancyFilterParams
    {
        public string Lang { get; set; } = "az";
        public int? CityId { get; set; }
        public int? CategoryId { get; set; }
        public Dictionary<string, string>? Filters { get; set; } = new();
        public string? Search { get; set; }
        public bool? IsPremium { get; set; }
        public int? CompanyId { get; set; }
        public float? MinSalary { get; set; }
        public float? MaxSalary { get; set; }

        public int Take { get; set; } = 10;
        public string? SortBy { get; set; }
    }