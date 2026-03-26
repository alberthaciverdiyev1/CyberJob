namespace CyberJob.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Icon { get; set; }

        public int? ParentId { get; set; }

        public int VacancyCount { get; set; } = 10;

        public List<CategoryDto> SubCategories { get; set; } = new();

        public DateTime? CreatedAt { get; set; }
    }
}