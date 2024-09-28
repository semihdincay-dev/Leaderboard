using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaderboard.Core.Entities
{
  public class Score : BaseEntity
  {
    public Guid UserId { get; set; }
    public int Points { get; set; }
    public DateTime AchievedDate { get; set; }

    public User User { get; set; }
  }
}
