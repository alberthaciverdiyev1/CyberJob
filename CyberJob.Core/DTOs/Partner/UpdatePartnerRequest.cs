namespace CyberJob.Core.DTOs.Partner;

public record UpdatePartnerRequest(int Id, string? Name, string? Link, Stream? Image);