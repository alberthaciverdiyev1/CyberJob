using CyberJob.DTOs;
using CyberJob.Models;

namespace CyberJob.ViewModels;

public class CompanyDetailsVM
{
    
    public CompanyDetailsDto? Company { get; set; } = new();
    public IEnumerable<City>?  Cities { get; set; } = new List<City>();
    public IEnumerable<FilterGroupDto> Filters { get; set; } = new List<FilterGroupDto>();
    
}