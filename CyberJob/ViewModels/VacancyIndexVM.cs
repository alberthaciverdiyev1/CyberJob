using CyberJob.DTOs;
using CyberJob.Models;

namespace CyberJob.ViewModels
{
    public class VacancyIndexVM
    {
        public IEnumerable<VacancyListDto> Vacancies { get; set; } = Enumerable.Empty<VacancyListDto>();
        public IEnumerable<BannerDto> Banners { get; set; } = Enumerable.Empty<BannerDto>();

        public IEnumerable<FilterGroupDto> Filters { get; set; } = Enumerable.Empty<FilterGroupDto>();
        public IEnumerable<CategoryDto> Categories { get; set; } = Enumerable.Empty<CategoryDto>();
        public int TotalCount { get; set; }
        public int ExpiredCount { get; set; }
    }
}
