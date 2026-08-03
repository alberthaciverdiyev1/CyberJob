using CyberJob.Database;
using CyberJob.Services;
using CyberJob.Helpers;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OutputCaching;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var isDevelopment = builder.Environment.IsDevelopment();

builder.Services.AddControllersWithViews();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
    options.MimeTypes = Microsoft.AspNetCore.ResponseCompression.ResponseCompressionDefaults.MimeTypes
        .Concat(["text/html", "application/json", "text/css", "application/javascript", "image/svg+xml"]);
});
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(5);
    options.AddBasePolicy(b => b.Cache().SetVaryByQuery("*").SetVaryByHeader("HX-Request").VaryByValue(ctx => new KeyValuePair<string, string>("lang", ctx.Items["lang"]?.ToString() ?? "az")).Tag("all"));
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    // options.Cookie.SecurePolicy = isDevelopment ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
});

builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        // options.Cookie.SecurePolicy = isDevelopment ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .WithMethods("GET", "POST")
              .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("SubscribePolicy", config =>
    {
        config.PermitLimit = 5;
        config.Window = TimeSpan.FromMinutes(1);
        config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        config.QueueLimit = 0;
    });

    options.AddFixedWindowLimiter("GlobalPolicy", config =>
    {
        config.PermitLimit = 100;
        config.Window = TimeSpan.FromMinutes(1);
    });
});

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(
        Path.Combine(builder.Environment.ContentRootPath, ".dataprotection-keys")))
    .SetApplicationName("CyberJob");

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always;
});

builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
    options.IncludeSubDomains = true;
    options.Preload = true;
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CyberJobConnection"))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
builder.Services.AddScoped<CyberJob.Helpers.SettingHelper>();
builder.Services.AddScoped<LanguageService>();
builder.Services.AddScoped<TranslationService>();
builder.Services.AddScoped<Localizer>();

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
builder.Services.AddScoped<SubscribeService>();
builder.Services.AddScoped<SubscriptionPlanService>();
builder.Services.AddScoped<LegalTermAndUserAgreementService>();

var app = builder.Build();

CyberJob.Helpers.UrlHelper.Initialize(app.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/not-found");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseRouting();

app.UseCookiePolicy();
app.UseCors("DefaultPolicy");
app.UseRateLimiter();

app.UseMiddleware<CyberJob.Middleware.SecurityHeadersMiddleware>();
app.UseMiddleware<CyberJob.Middleware.LanguageMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.UseOutputCache();

app.MapGet("/clear-cache", async (IOutputCacheStore cache) =>
{
    await cache.EvictByTagAsync("all", default);
    return Results.Ok(new { message = "Cache cleared" });
});

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();