using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

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



//privremeno ovdje samo za lab1, ispisuje u konzolu za testiranje upita
var user1 = new User { Id = 1, Username = "Đuro", DateJoined = DateTime.Now };
var user2 = new User { Id = 2, Username = "Pero", DateJoined = DateTime.Now };
var user3 = new User { Id = 3, Username = "Marko", DateJoined = DateTime.Now };

var tab1 = new Tab
{
    Id = 1,
    Title = "Neka pjesma",
    Artist = "Neki bend",
    CreatorId = 1,
    Creator = user1,
    DateCreated = DateTime.Now,
    StringTuning = "E-A-D-G-H-E",
    Difficulty = Difficulty.Intermediate,
    BPM = 120,
    Measures = new List<TabMeasure>
    {
        new TabMeasure
        {
            CapitalNumber = 1,
            TimeSignature = (4,4),
            Columns = new List<TabColumn>
            {
                new TabColumn
                {
                    CapitalNumber = 1,
                    ColumnDuration = new Duration{Base=4, IsDotted=false},
                    Notes = new List<TabNote>
                    {
                        new TabNote { String = 3, Fret = 0, },
                        new TabNote { String = 4, Fret = 0, }
                    }
                },
            new TabColumn
                {
                    CapitalNumber = 2,
                    ColumnDuration = new Duration{Base=4, IsDotted=false},
                    Notes =new List<TabNote>
                    {
                        new TabNote { String = 3, Fret = 3, },
                        new TabNote { String = 4, Fret = 3, }
                    }
                },
            new TabColumn
                {
                    CapitalNumber = 3,
                    ColumnDuration = new Duration{Base=4, IsDotted=false},
                    Notes =new List<TabNote>
                    {
                        new TabNote { String = 3, Fret = 5, },
                        new TabNote { String = 4, Fret = 5, }
                    }
                }
            }
        }
    }
};

var tab2 = new Tab
{
    Id = 2,
    Title = "Song 2",
    Artist = "Neki bend",
    CreatorId = 1,
    Creator = user1,
    DateCreated = DateTime.Now,
    StringTuning = "E-A-D-G-H-E",
    Difficulty = Difficulty.Easy,
    BPM = 60
};

var tab3 = new Tab
{
    Id = 3,
    Title = "Song 3",
    Artist = "Artist 2",
    CreatorId = 2,
    Creator = user2,
    StringTuning = "D-A-D-G-H-E",
    DateCreated = DateTime.Now,
    Difficulty = Difficulty.Intermediate,
    BPM = 120
};

List<Tab> tabs = new List<Tab> { tab1, tab2, tab3 };

var intermediateTabs = tabs.Where(t => t.Difficulty == Difficulty.Intermediate);
var nekiBendTabs = tabs.Where(t => t.Artist == "Neki bend");
var sixtyBpmTabs = tabs.Where(t => t.BPM == 60);

Console.WriteLine("Intermediate tabs:");
foreach (var tab in intermediateTabs)
{
    Console.WriteLine(tab.Title);
}

Console.WriteLine("Neki bend tabs:");
foreach (var tab in nekiBendTabs)
{
    Console.WriteLine(tab.Title);
}

Console.WriteLine("60 BPM tabs:");
foreach (var tab in sixtyBpmTabs)
{
    Console.WriteLine(tab.Title);
}

app.Run();