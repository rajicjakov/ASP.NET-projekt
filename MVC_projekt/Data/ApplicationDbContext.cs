using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC_projekt.Models;

namespace MVC_projekt.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tab> Tabs => Set<Tab>();
        public DbSet<TabMeasure> TabMeasures => Set<TabMeasure>();
        public DbSet<TabColumn> TabColumns => Set<TabColumn>();
        public DbSet<TabNote> TabNotes => Set<TabNote>();
        public DbSet<Duration> Durations => Set<Duration>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("Users");
                b.Property(u => u.UserName).HasColumnName("Username").HasMaxLength(100).IsRequired();
                b.Property(u => u.NormalizedUserName).HasColumnName("NormalizedUsername").HasMaxLength(100);
                b.Property(u => u.Email).HasMaxLength(256).IsRequired();
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);
                b.Property(u => u.PasswordHash).HasMaxLength(256);
                b.Property(u => u.DateJoined).IsRequired();
                b.Property(u => u.Role).IsRequired();
            });

            modelBuilder.Entity<IdentityRole<int>>(b =>
            {
                b.ToTable("Roles");
                b.Property(r => r.Name).HasMaxLength(256);
                b.Property(r => r.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<IdentityUserRole<int>>(b => b.ToTable("UserRoles"));
            modelBuilder.Entity<IdentityUserClaim<int>>(b => b.ToTable("UserClaims"));
            modelBuilder.Entity<IdentityUserLogin<int>>(b => b.ToTable("UserLogins"));
            modelBuilder.Entity<IdentityRoleClaim<int>>(b => b.ToTable("RoleClaims"));
            modelBuilder.Entity<IdentityUserToken<int>>(b => b.ToTable("UserTokens"));

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
