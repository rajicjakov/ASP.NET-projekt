using Microsoft.EntityFrameworkCore;
using MVC_projekt.Data;
using MVC_projekt.Models;

namespace MVC_projekt.Services
{
    public class DbTabRepository : ITabRepository
    {
        private readonly ApplicationDbContext _context;

        public DbTabRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Tab> GetAllTabs()
        {
            return _context.Tabs
                .Include(t => t.Creator)
                .Include(t => t.Measures)
                    .ThenInclude(m => m.Columns)
                        .ThenInclude(c => c.Notes)
                .Include(t => t.Measures)
                    .ThenInclude(m => m.Columns)
                        .ThenInclude(c => c.ColumnDuration)
                .ToList();
        }

        public Tab GetTabById(int id)
        {
            return _context.Tabs
                .Include(t => t.Creator)
                .Include(t => t.Measures)
                    .ThenInclude(m => m.Columns)
                        .ThenInclude(c => c.Notes)
                .Include(t => t.Measures)
                    .ThenInclude(m => m.Columns)
                        .ThenInclude(c => c.ColumnDuration)
                .FirstOrDefault(t => t.Id == id)!;
        }

        public void AddTab(Tab tab)
        {
            _context.Tabs.Add(tab);
            _context.SaveChanges();
        }

        public void UpdateTab(Tab tab)
        {
            var existing = _context.Tabs.Find(tab.Id);
            if (existing == null)
            {
                return;
            }

            existing.Title = tab.Title;
            existing.Artist = tab.Artist;
            existing.StringTuning = tab.StringTuning;
            existing.BPM = tab.BPM;
            existing.Difficulty = tab.Difficulty;

            _context.SaveChanges();
        }

        public void DeleteTabById(int id)
        {
            var tab = _context.Tabs.Find(id);
            if (tab != null)
            {
                _context.Tabs.Remove(tab);
                _context.SaveChanges();
            }
        }

        public List<Tab> GetTabsByCreator(int creatorId)
        {
            return _context.Tabs
                .Where(t => t.CreatorId == creatorId)
                .Include(t => t.Creator)
                .Include(t => t.Measures)
                    .ThenInclude(m => m.Columns)
                        .ThenInclude(c => c.Notes)
                .Include(t => t.Measures)
                    .ThenInclude(m => m.Columns)
                        .ThenInclude(c => c.ColumnDuration)
                .ToList();
        }
    }
}
