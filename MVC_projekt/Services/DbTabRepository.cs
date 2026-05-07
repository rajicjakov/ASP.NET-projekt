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
    }
}
