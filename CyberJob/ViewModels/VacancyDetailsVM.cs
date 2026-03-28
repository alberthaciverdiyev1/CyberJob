using CyberJob.DTOs;

namespace CyberJob.ViewModels
{
    public class VacancyDetailsVM
    {
        public VacancyDetailDto Vacancy { get; set; } = new VacancyDetailDto();
        public IEnumerable<VacancyListDto> SimilarVacancies { get; set; } = Enumerable.Empty<VacancyListDto>();
    }
}
