namespace CyberJob.DTOs
{
    public class VacancyDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Requirements { get; set; }
        public string? Salary { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public string? Email { get; set; }
        public int ViewCount { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPremium { get; set; }
        public string? BannerImage { get; set; }
        public bool IsBringTop { get; set; }

        public string? City { get; set; }
        public string? Category { get; set; }

        public int CategoryId { get; set; }

        public CompanyDetailDto Company { get; set; } = new();
        public List<FilterDetailDto> Filters { get; set; } = new();
    }

    public class CompanyDetailDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Logo { get; set; }
        public string? About { get; set; }
        
        public string? Email { get; set; }
        public bool IsVerified { get; set; }
        public string? BannerImage { get; set; }
    }

    public class FilterDetailDto
    {
        public int Id { get; set; }
        public string? Key { get; set; }
        public string? Name { get; set; }
        public string? ParentName { get; set; }
    }
}