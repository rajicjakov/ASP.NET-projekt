using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_projekt.Models;
using MVC_projekt.Services;
using MVC_projekt.Data;

namespace MVC_projekt.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class EditorController : Controller
    {
        private readonly ITabRepository _tabRepository;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public EditorController(ITabRepository tabRepository, UserManager<User> userManager, ApplicationDbContext context)
        {
            _tabRepository = tabRepository;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var tab = _tabRepository.GetTabById(id);
            if (tab == null)
            {
                return NotFound();
            }

            return View("Create", tab);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, string title, string artist, string tuning, int? bpm, Difficulty difficulty, string dateCreated)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(artist))
            {
                ModelState.AddModelError(string.Empty, "Title and Artist are required.");
                var model = id.HasValue
                    ? _tabRepository.GetTabById(id.Value)
                    : new Tab { Title = title, Artist = artist, StringTuning = tuning ?? string.Empty, BPM = bpm ?? 0, Difficulty = difficulty };
                return View("Create", model);
            }

            DateTime parsedDate = DateTime.UtcNow;
            if (!string.IsNullOrWhiteSpace(dateCreated) && !DateTime.TryParse(dateCreated, out parsedDate))
            {
                parsedDate = DateTime.UtcNow;
            }

            if (id.HasValue)
            {
                var existingTab = _tabRepository.GetTabById(id.Value);
                if (existingTab == null)
                {
                    return NotFound();
                }

                existingTab.Title = title;
                existingTab.Artist = artist;
                existingTab.StringTuning = tuning ?? string.Empty;
                existingTab.BPM = bpm ?? 0;
                existingTab.Difficulty = difficulty;
                existingTab.DateCreated = parsedDate;

                _tabRepository.UpdateTab(existingTab);

                return RedirectToAction("Details", "Browse", new { id = existingTab.Id });
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            var tab = new Tab
            {
                Title = title,
                Artist = artist,
                StringTuning = tuning ?? string.Empty,
                BPM = bpm ?? 0,
                Difficulty = difficulty,
                DateCreated = parsedDate,
                CreatorId = currentUser.Id
            };

            _tabRepository.AddTab(tab);

            return RedirectToAction("Details", "Browse", new { id = tab.Id });
        }

        [HttpGet("api/artists")]
        public JsonResult GetArtistsAutocomplete(string term)
        {
            var artists = _context.Tabs
                .Where(t => string.IsNullOrEmpty(term) || t.Artist.Contains(term))
                .Select(t => t.Artist)
                .Distinct()
                .OrderBy(a => a)
                .Take(10)
                .ToList();

            return Json(artists);
        }

        [HttpGet("api/tunings")]
        public JsonResult GetTuningsAutocomplete(string term)
        {
            var tunings = _context.Tabs
                .Where(t => !string.IsNullOrEmpty(t.StringTuning))
                .Where(t => string.IsNullOrEmpty(term) || t.StringTuning.Contains(term))
                .Select(t => t.StringTuning)
                .Distinct()
                .OrderBy(tu => tu)
                .Take(10)
                .ToList();

            return Json(tunings);
        }

        [HttpGet("api/titles")]
        public JsonResult GetTitlesAutocomplete(string term)
        {
            var titles = _context.Tabs
                .Where(t => string.IsNullOrEmpty(term) || t.Title.Contains(term))
                .Select(t => t.Title)
                .Distinct()
                .OrderBy(ti => ti)
                .Take(10)
                .ToList();

            return Json(titles);
        }
    }
}