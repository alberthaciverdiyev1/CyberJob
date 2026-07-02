using CyberJob.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberJob.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Category> Categories { get; set; }

    public DbSet<CompanyCategory> CompanyCategories { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Partner> Partners { get; set; }
    public DbSet<Faq> Faqs { get; set; }
    public DbSet<Filter> Filters { get; set; }
    
    public DbSet<Setting> Settings { get; set; }
    public DbSet<Vacancy> Vacancies { get; set; }
    public DbSet<VacancyFilter> VacancyFilters { get; set; }
    public DbSet<Subscribe> Subscribes { get; set; }
    public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
    public DbSet<SubscriptionPlanOption> SubscriptionPlanOptions { get; set; }
    public DbSet<LegalTermAndUserAgreement> LegalTermAndUserAgreements { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}