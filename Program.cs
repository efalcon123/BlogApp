using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using BlogApp.Data;
using BlogApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationBlogContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ApplicationBlogContext") 
                      ?? throw new InvalidOperationException("Connection string 'ApplicationBlogContext' not found.")));

// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/"; 
        options.AccessDeniedPath = "/AccessDenied"; 
        options.LogoutPath = "/";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationBlogContext>();

    // Ensure database is created
    context.Database.EnsureCreated();

    // Seed data
    if (!context.User.Any())
    {
        var adminPasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!");
        var editorPasswordHash = BCrypt.Net.BCrypt.HashPassword("Editor123!");

        context.User.AddRange(
            new User
            {
                Id = new Guid("4e2357c8-08b8-4b1d-bde7-b3389fd17690"),
                Email = "admin@blogapp.com",
                Password = adminPasswordHash,
                Role = "Admin"
            },
            new User
            {
                Id = new Guid("aeb357c8-08b8-4b1d-bde7-b3389fd17666"),
                Email = "editor@blogapp.com",
                Password = editorPasswordHash,
                Role = "Editor"
            }
        );

        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
