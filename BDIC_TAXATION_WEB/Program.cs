using BDIC_TAXATION_ACCESS.Data;
using BDIC_TAXATION_ACCESS.Repository;
using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_ACCESS.SystemRepos;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_WEB.HelperClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(h =>
{
    h.UseMySql(connection, ServerVersion.AutoDetect(connection));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMailSender, MailSender>();
builder.Services.AddScoped<IFileLoader, FileLoader>();

builder.Services.Configure<MailConfig>(builder.Configuration.GetSection("MailConfig"));
builder.Services.AddSingleton(q=>q.GetRequiredService<IOptions<MailConfig>>().Value);
builder.Services.AddSingleton<HtmlTemplateService>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
{
    //Accont Lockout configuration
    //Options.Lockout.MaxFailedAccessAttempts = 3;
    //Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(35800);
    // SETTING UP YOUR CUSTOM PASSWORD REQUIREMENTS
    Options.Password.RequiredLength = 3;
    Options.Password.RequiredUniqueChars = 0;
    Options.Password.RequireNonAlphanumeric = false;
    Options.Password.RequireDigit = false;
    Options.Password.RequireLowercase = false;
    Options.Password.RequireUppercase = false;
    Options.User.AllowedUserNameCharacters = "/abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    //Email confirmation configuration
    //Options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

// Register HttpClient
builder.Services.AddHttpClient<ITransactionService, TransactionService>();

// Register the TransactionService with DI
builder.Services.AddScoped<ITransactionService, TransactionService>(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new TransactionService(httpClient, configuration);
});


// Add health checks (recommended for 8.x)
builder.Services.AddHealthChecks();

builder.Services.ConfigureApplicationCookie(a =>
{
    a.LoginPath = $"/Account/Login";
    a.LogoutPath = $"/Account/Logout";
    a.AccessDeniedPath = $"/Account/AccessDenied";
});

//CREATE A METHOD FOR EXECUTING MIGRATIONS
void RunMigrations(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Handle exceptions appropriately (e.g., logging)
        Console.WriteLine($"An error occurred during migration: {ex.Message}");
        // Optionally, rethrow the exception or handle it as needed.
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//CALLING MIGRATION YOUR METHOD
RunMigrations(app);
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
