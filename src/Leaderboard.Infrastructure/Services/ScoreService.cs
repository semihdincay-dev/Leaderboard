using Leaderboard.Core.Entities;
using Leaderboard.Core.Interfaces;
using Leaderboard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Infrastructure.Services
{
  public class ScoreService : IScoreService
  {

    private readonly LeaderboardContext _context;
    private readonly ICacheService _cacheService;

    public ScoreService(LeaderboardContext context, ICacheService cacheService)
    {
      _context = context;
      _cacheService = cacheService;
    }

    public async Task AddScoreAsync(Score score)
    {
      using var transaction = await _context.Database.BeginTransactionAsync();

      try
      {
        _context.Scores.Add(score);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        var topScores = await GetTopScoresAsync(100);
        await CacheTopScoresAsync(topScores);

      }
      catch (Exception ex)
      {
        await transaction.RollbackAsync();
        throw;
      }
    }

    public async Task<List<Score>> GetTopScoresAsync(int count)
    {
      var cachedScores = await _cacheService.GetAsync<List<Score>>("Top100Scores");
      if (cachedScores != null)
      {
        return cachedScores.Take(count).ToList();
      }

      var topScores = await _context.Scores
        .Include(s => s.User)
        .OrderByDescending(s => s.Points)
        .ThenBy(s => s.User.RegistrationDate)
        .ThenBy(s => s.User.Level)
        .ThenBy(s => s.User.TrophyCount)
        .Take(count)
        .ToListAsync();

      await CacheTopScoresAsync(topScores);

      return topScores;
    }

    public async Task CacheTopScoresAsync(List<Score> topScores)
    {
      await _cacheService.SetAsync("Top100Scores", topScores, TimeSpan.FromSeconds(1));
    }


  }
}
