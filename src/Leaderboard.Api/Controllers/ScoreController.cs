using AutoMapper;
using Leaderboard.Api.Models.DTOs;
using Leaderboard.Core.Entities;
using Leaderboard.Core.Interfaces;
using Leaderboard.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Api.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class ScoreController : ControllerBase
  {
    private readonly IScoreService _scoreService;
    private readonly IMapper _mapper;
    private readonly ILogger<ScoreController> _logger;

    public ScoreController(IMapper mapper, IScoreService scoreService, ILogger<ScoreController> logger)
    {
      _scoreService = scoreService;
      _mapper = mapper;
      _logger = logger;
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitScore([FromBody] Score score)
    {
      _logger.LogInformation("Skor gönderildi: {Points}", score.Points);

      try
      {
        await _scoreService.AddScoreAsync(score);

        var topScores = await _scoreService.GetTopScoresAsync(100);
        var topScoresDTO = _mapper.Map<List<ScoreDTO>>(topScores);

        _logger.LogInformation("Skorlar başarıyla kaydedildi");
        return Ok(topScoresDTO);
      }
      catch (Exception ex)
      {
        _logger.LogError("Skor gönderiminde hata oluştu: {message}", ex.Message);
        return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
      }
    }

    [HttpGet("top/{count}")]
    public async Task<IActionResult> GetTopScores(int count)
    {
      _logger.LogInformation("Top {Count} skorlar isteniyor", count);

      var topScores = await _scoreService.GetTopScoresAsync(count);
      var topScoresDTO = _mapper.Map<List<ScoreDTO>>(topScores);

      return Ok(topScoresDTO);
    }
  }
}
