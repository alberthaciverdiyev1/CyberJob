namespace CyberJob.Core.DTOs.Category;

public record UpdateCategoryRequest(string Name, string Icon, int? ParentId);