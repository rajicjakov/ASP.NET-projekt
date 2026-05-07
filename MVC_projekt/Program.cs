using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MVC_projekt.Data;
using MVC_projekt.Models;
using MVC_projekt.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ITabRepository, DbTabRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    if (!context.Tabs.Any())
    {
        var user1 = new User
        {
            Username = "Đuro",
            Email = "djuro@example.com",
            PasswordHash = PasswordHasher.Hash("Password123!"),
            DateJoined = DateTime.UtcNow.AddDays(-15),
            Role = UserRole.Regular
        };

        var user2 = new User
        {
            Username = "Pero",
            Email = "pero@example.com",
            PasswordHash = PasswordHasher.Hash("Password123!"),
            DateJoined = DateTime.UtcNow.AddDays(-25),
            Role = UserRole.Regular
        };

        var user3 = new User
        {
            Username = "Ana",
            Email = "ana@example.com",
            PasswordHash = PasswordHasher.Hash("Admin123!"),
            DateJoined = DateTime.UtcNow.AddDays(-5),
            Role = UserRole.Admin
        };

        var tabs = new List<Tab>
        {
            new Tab
            {
                Title = "Sunset Road",
                Artist = "Evening Drive",
                Creator = user1,
                DateCreated = DateTime.UtcNow.AddDays(-10),
                StringTuning = "E-A-D-G-B-E",
                BPM = 95,
                Difficulty = Difficulty.Easy
            },
            new Tab
            {
                Title = "Midnight Pulse",
                Artist = "Neon Strings",
                Creator = user2,
                DateCreated = DateTime.UtcNow.AddDays(-8),
                StringTuning = "D-A-D-G-B-E",
                BPM = 120,
                Difficulty = Difficulty.Intermediate
            },
            new Tab
            {
                Title = "Storm Chaser",
                Artist = "Thunder Guild",
                Creator = user3,
                DateCreated = DateTime.UtcNow.AddDays(-3),
                StringTuning = "E-A-D-G-H-E",
                BPM = 140,
                Difficulty = Difficulty.Hard
            }
        };

        context.Users.AddRange(user1, user2, user3);
        context.Tabs.AddRange(tabs);
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();