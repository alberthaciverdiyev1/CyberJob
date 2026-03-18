using System.ComponentModel.DataAnnotations.Schema;

namespace CyberJob.Models;

[Table("company_categories")]
public class CompanyCategory
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")] 
    public string Name { get; set; }
}