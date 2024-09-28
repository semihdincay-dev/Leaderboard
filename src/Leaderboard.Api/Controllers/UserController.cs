using Leaderboard.Api.Models;
using Leaderboard.Core.Entities;
using Leaderboard.Core.Services;
using Leaderboard.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly LeaderboardContext _context;
    private readonly AuthenticationService _authService;

    public UserController(LeaderboardContext context, AuthenticationService authService)
    {
      _context = context;
      _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
      _context.Users.Add(user);
      await _context.SaveChangesAsync();
      return Ok(user);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);
      if (user == null)
      {
        return Unauthorized("Kullanıcı adı veya şifre yanlış");
      }

      var token = _authService.GenerateJwtToken(user);
      return Ok(token);
    }

    public async Task<IActionResult> GetUserById(Guid id)
    {
      var user = await _context.Users.FindAsync(id);
      if (user == null)
      {
        return NotFound();
      }
      return Ok(user);
    }
  }
}
