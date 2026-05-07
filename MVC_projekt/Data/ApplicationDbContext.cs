using Microsoft.EntityFrameworkCore;
using MVC_projekt.Models;

namespace MVC_projekt.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Tab> Tabs => Set<Tab>();
        public DbSet<TabMeasure> TabMeasures => Set<TabMeasure>();
        public DbSet<TabColumn> TabColumns => Set<TabColumn>();
        public DbSet<TabNote> TabNotes => Set<TabNote>();
        public DbSet<Duration> Durations => Set<Duration>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tab>()
                .HasOne(t => t.Creator)
                .WithMany(u => u.Tabs)
                .HasForeignKey(t => t.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TabMeasure>()
                .HasOne(m => m.Tab)
                .WithMany(t => t.Measures)
                .HasForeignKey(m => m.TabId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TabColumn>()
                .HasOne(c => c.TabMeasure)
                .WithMany(m => m.Columns)
                .HasForeignKey(c => c.TabMeasureId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TabNote>()
                .HasOne(n => n.TabColumn)
                .WithMany(c => c.Notes)
                .HasForeignKey(n => n.TabColumnId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TabColumn>()
                .HasOne(c => c.ColumnDuration)
                .WithOne(d => d.TabColumn)
                .HasForeignKey<Duration>(d => d.TabColumnId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
