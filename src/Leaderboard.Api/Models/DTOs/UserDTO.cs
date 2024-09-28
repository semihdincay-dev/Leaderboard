namespace Leaderboard.Api.Models.DTOs
{
  public class UserDTO : BaseDTO
  {
        public string Username { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int Level { get; set; }
        public int TrophyCount { get; set; }
    }
}
