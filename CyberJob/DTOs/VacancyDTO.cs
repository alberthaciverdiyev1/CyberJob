namespace CyberJob.DTOs
{
    public class VacancyListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Salary { get; set; }
        public int ViewCount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsPremium { get; set; }
        public string? CityName { get; set; }
        public VacancyCompanyDto Company { get; set; }
    }

    public class VacancyCompanyDto
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        public bool IsVerified { get; set; }
    }
}
