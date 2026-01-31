namespace CyberJob.Core.DTOs.Category;

public record CategoryResponse(int Id, string Name, string Icon, int? ParentId, DateTime CreatedAt, DateTime UpdatedAt);