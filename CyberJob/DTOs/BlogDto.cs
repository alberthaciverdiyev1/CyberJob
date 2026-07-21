namespace CyberJob.DTOs;

public class BlogListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Image { get; set; }
    public int ReadCount { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
}

public class BlogDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Image { get; set; }
    public int ReadCount { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
}
