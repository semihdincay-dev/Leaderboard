using Leaderboard.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Infrastructure.Data
{
  public class LeaderboardContext : DbContext
  {
    public LeaderboardContext(DbContextOptions<LeaderboardContext> options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Score> Scores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        optionsBuilder.UseNpgsql("Host=localhost;Database=LeaderboardDB;Username=postgres;Password=123");
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<User>()
        .HasMany(u => u.Scores)
        .WithOne(s => s.User)
        .HasForeignKey(s => s.UserId)
        .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<Score>()
        .HasOne(s => s.User)
        .WithMany(u => u.Scores)
        .HasForeignKey(s => s.UserId);
    }
  }
}
