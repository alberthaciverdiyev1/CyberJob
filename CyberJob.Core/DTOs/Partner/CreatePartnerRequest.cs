namespace CyberJob.Core.DTOs.Partner;

public record CreatePartnerRequest(string? Name, string? Link, Stream? Image);