using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaderboard.Core.Entities
{
  public class User : BaseEntity
  {
    public string Username { get; set; }
    public string Password { get; set; }
    public DateTime RegistrationDate { get; set; }
    public int Level { get; set; }
    public int TrophyCount { get; set; }
        public ICollection<Score> Scores { get; set; }

    }
}
