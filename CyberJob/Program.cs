using CyberJob.Database;
using CyberJob.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CyberJobConnection"))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
builder.Services.AddScoped<CyberJob.Helpers.SettingHelper>();

builder.Services.AddScoped<BannerService>();  
builder.Services.AddScoped<VacancyService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<CityService>();
builder.Services.AddScoped<FilterService>();
builder.Services.AddScoped<PartnerService>();
builder.Services.AddScoped<FaqService>();
builder.Services.AddScoped<StatisticsService>();

var app = builder.Build();

CyberJob.Helpers.UrlHelper.Initialize(app.Configuration);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();