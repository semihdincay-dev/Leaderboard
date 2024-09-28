using Leaderboard.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaderboard.Core.Interfaces
{
  public interface IScoreService
  {
    Task AddScoreAsync(Score score);
    Task<List<Score>> GetTopScoresAsync(int count);
    Task CacheTopScoresAsync(List<Score> topScores);
  }
}
