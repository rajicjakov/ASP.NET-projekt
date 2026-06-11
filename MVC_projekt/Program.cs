using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC_projekt.Data;
using MVC_projekt.Models;
using MVC_projekt.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login/Login";
    options.LogoutPath = "/Login/Logout";
});

// External authentication (Google)
builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });

builder.Services.AddScoped<ITabRepository, DbTabRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    context.Database.Migrate();

    if (!context.Users.Any())
    {
        var user1 = new User
        {
            Username = "Đuro",
            Email = "djuro@example.com",
            DateJoined = DateTime.UtcNow.AddDays(-15),
            Role = UserRole.Regular
        };

        var user2 = new User
        {
            Username = "Pero",
            Email = "pero@example.com",
            DateJoined = DateTime.UtcNow.AddDays(-25),
            Role = UserRole.Regular
        };

        var user3 = new User
        {
            Username = "Ana",
            Email = "ana@example.com",
            DateJoined = DateTime.UtcNow.AddDays(-5),
            Role = UserRole.Admin
        };

        var createResult1 = userManager.CreateAsync(user1, "Password123!").GetAwaiter().GetResult();
        var createResult2 = userManager.CreateAsync(user2, "Password123!").GetAwaiter().GetResult();
        var createResult3 = userManager.CreateAsync(user3, "Admin123!").GetAwaiter().GetResult();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();