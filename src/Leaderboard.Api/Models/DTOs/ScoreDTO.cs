namespace Leaderboard.Api.Models.DTOs
{
  public class ScoreDTO : BaseDTO
  {
        public Guid UserId { get; set; }
        public int Points { get; set; }
        public DateTime AchievedDate { get; set; }
        public UserDTO User { get; set; }
    }
}
