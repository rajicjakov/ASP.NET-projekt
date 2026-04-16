using Microsoft.AspNetCore.Mvc;
using MVC_projekt.Models;
using MVC_projekt.Services;

namespace MVC_projekt.Controllers
{
    public class BrowseController : Controller
    {
        private readonly ITabRepository _tabRepository;

        public BrowseController(ITabRepository tabRepository)
        {
            _tabRepository = tabRepository;
        }

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

        public IActionResult Details(int id)
        {
            var tab = _tabRepository.GetTabById(id);
            if (tab == null)
            {
                return NotFound();
            }

            return View(tab);
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
    }
}