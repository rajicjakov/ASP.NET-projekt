using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_projekt.Models;
using MVC_projekt.Services;

namespace MVC_projekt.Controllers
{
    [Route("[controller]")]
    public class BrowseController : Controller
    {
        private readonly ITabRepository _tabRepository;
        private readonly UserManager<User> _userManager;

        public BrowseController(ITabRepository tabRepository, UserManager<User> userManager)
        {
            _tabRepository = tabRepository;
            _userManager = userManager;
        }

        [HttpGet("")]
        public IActionResult Browse(string searchTerm = "", string[] selectedDifficulties = null, string[] selectedTunings = null)
        {
            var allTabs = _tabRepository.GetAllTabs();
            var filteredBySearch = ApplySearch(allTabs, searchTerm);

            var difficulties = Enum.GetValues<Difficulty>().Cast<Difficulty>().ToList();
            var difficultyCounts = difficulties.ToDictionary(
                difficulty => difficulty,
                difficulty => filteredBySearch.Count(t => t.Difficulty == difficulty)
            );

            var tuningCounts = filteredBySearch
                .GroupBy(t => string.IsNullOrEmpty(t.StringTuning) ? "Unknown" : t.StringTuning)
                .OrderByDescending(g => g.Count())
                .ThenBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Count());

            var selectedDifficultySet = (selectedDifficulties ?? Array.Empty<string>())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            var selectedTuningSet = (selectedTunings ?? Array.Empty<string>())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var resultTabs = filteredBySearch;
            if (selectedDifficultySet.Any())
            {
                resultTabs = resultTabs.Where(t => selectedDifficultySet.Contains(t.Difficulty.ToString())).ToList();
            }

            if (selectedTuningSet.Any())
            {
                resultTabs = resultTabs.Where(t => selectedTuningSet.Contains(t.StringTuning ?? "Unknown")).ToList();
            }

            ViewBag.IsAdmin = User.IsInRole("Admin");
            ViewBag.CurrentUserId = GetCurrentUserId();

            var model = new BrowseViewModel
            {
                SearchTerm = searchTerm,
                Tabs = resultTabs,
                AllDifficulties = difficulties,
                AllTunings = tuningCounts.Keys.ToList(),
                DifficultyCounts = difficultyCounts,
                TuningCounts = tuningCounts,
                SelectedDifficulties = selectedDifficultySet.ToList(),
                SelectedTunings = selectedTuningSet.ToList()
            };

            return View(model);
        }

        [Authorize]
        [HttpGet("MyTabs")]
        public IActionResult MyTabs()
        {
            var currentUserId = GetCurrentUserId();
            var tabs = _tabRepository.GetTabsByCreator(currentUserId);

            ViewBag.IsAdmin = User.IsInRole("Admin");
            ViewBag.CurrentUserId = currentUserId;

            var model = new BrowseViewModel
            {
                SearchTerm = string.Empty,
                Tabs = tabs,
                AllDifficulties = Enum.GetValues<Difficulty>().Cast<Difficulty>().ToList(),
                AllTunings = tabs.GroupBy(t => string.IsNullOrEmpty(t.StringTuning) ? "Unknown" : t.StringTuning)
                    .OrderByDescending(g => g.Count())
                    .ThenBy(g => g.Key)
                    .Select(g => g.Key)
                    .ToList(),
                DifficultyCounts = tabs.GroupBy(t => t.Difficulty).ToDictionary(g => g.Key, g => g.Count()),
                TuningCounts = tabs.GroupBy(t => string.IsNullOrEmpty(t.StringTuning) ? "Unknown" : t.StringTuning)
                    .ToDictionary(g => g.Key, g => g.Count()),
                SelectedDifficulties = new List<string>(),
                SelectedTunings = new List<string>()
            };

            return View("Browse", model);
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            var tab = _tabRepository.GetTabById(id);
            if (tab == null)
            {
                return NotFound();
            }

            ViewBag.IsAdmin = User.IsInRole("Admin");
            ViewBag.CurrentUserId = GetCurrentUserId();

            return View(tab);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _tabRepository.DeleteTabById(id);
            return RedirectToAction(nameof(Browse));
        }

        private static List<Tab> ApplySearch(List<Tab> tabs, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return tabs;
            }

            return tabs.Where(t =>
                (!string.IsNullOrEmpty(t.Title) && t.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(t.Artist) && t.Artist.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (t.Creator != null && !string.IsNullOrEmpty(t.Creator.Username) && t.Creator.Username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        private int GetCurrentUserId()
        {
            var userIdString = _userManager.GetUserId(User);
            return int.TryParse(userIdString, out var userId) ? userId : 0;
        }
    }
}