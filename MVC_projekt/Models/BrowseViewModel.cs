
namespace MVC_projekt.Models
{
    public class BrowseViewModel
    {
        public string SearchTerm { get; set; } = string.Empty;
        public List<Tab> Tabs { get; set; } = new List<Tab>();
        public List<Difficulty> AllDifficulties { get; set; } = new List<Difficulty>();
        public List<string> AllTunings { get; set; } = new List<string>();
        public Dictionary<Difficulty, int> DifficultyCounts { get; set; } = new Dictionary<Difficulty, int>();
        public Dictionary<string, int> TuningCounts { get; set; } = new Dictionary<string, int>();
        public List<string> SelectedDifficulties { get; set; } = new List<string>();
        public List<string> SelectedTunings { get; set; } = new List<string>();
    }
}