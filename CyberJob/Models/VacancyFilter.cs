using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberJob.Models;

[Table("vacancy_filters")]
public class VacancyFilter
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("vacancy_id")]
    public int VacancyId { get; set; }

    [Column("filter_id")]
    public int FilterId { get; set; }

    [ForeignKey("VacancyId")]
    public virtual Vacancy? Vacancy { get; set; }

    [ForeignKey("FilterId")]
    public virtual Filter? Filter { get; set; }
}