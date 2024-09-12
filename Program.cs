using EventEatsQuotify.ContextDBConfig;
using EventEatsQuotify.Interfaces;
using EventEatsQuotify.Models;
using EventEatsQuotify.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Retrieve the database connection string from appsettings.json
var dbConnection = builder.Configuration.GetConnectionString("dbConnection");

// Configure the DbContext to use SQL Server with the provided connection string
builder.Services.AddDbContext<EventEatsQuotifyDBContext>(options =>
    options.UseSqlServer(dbConnection));

// Add Identity services with the specified options and configure Entity Framework stores
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    // Allow spaces in the username
    options.User.AllowedUserNameCharacters += " ";
})
    .AddEntityFrameworkStores<EventEatsQuotifyDBContext>()
    .AddDefaultTokenProviders();

// Register the VendorService as a scoped service
builder.Services.AddScoped<VendorService>();

// Configure authentication with cookie authentication scheme
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// Configure authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Register the IEmailSender service and its implementation
builder.Services.Configure<EmailSettings>(options =>
{
    builder.Configuration.GetSection("EmailSettings").Bind(options);
    // Override with environment variables if available
    options.UserName = Environment.GetEnvironmentVariable("EMAIL_USER") ?? options.UserName;
    options.Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? options.Password;
});
builder.Services.AddScoped<IEmailSender, EmailSender>();

// Create the application
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Automatically apply any pending migrations to the database during startup
using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var context = services.GetRequiredService<EventEatsQuotifyDBContext>();
    context.Database.Migrate();

    // Seed admin account if not exists
    await SeedAdminAccountAsync(services);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map admin controller route
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{controller=Admin}/{action=Index}/{id?}"
);

app.Run();

// Method to seed the admin account
async Task SeedAdminAccountAsync(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Check if admin role exists, if not, create it
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // Check if admin user exists, if not, create it
    if (await userManager.FindByEmailAsync("admin@EEQ.com") == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@EEQ.com",
            // Add other properties as needed
        };

        var result = await userManager.CreateAsync(adminUser, "Admin@123");

        if (result.Succeeded)
        {
            // Assign admin role to the admin user
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
