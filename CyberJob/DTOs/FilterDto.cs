namespace CyberJob.DTOs
{
    public class FilterGroupDto
    {
        public int Id { get; set; }
        public string? Key { get; set; }
        public string? Name { get; set; } 

        public List<FilterOptionDto> Options { get; set; } = new();
    }

    public class FilterOptionDto
    {
        public int Id { get; set; }
        public string? Key { get; set; }
        public string? Name { get; set; }
        public int VacancyCount { get; set; }
    }
}