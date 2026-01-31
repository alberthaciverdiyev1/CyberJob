namespace CyberJob.Core.DTOs.Category;

public record CreateCategoryRequest(string Name, string Icon, int? ParentId);