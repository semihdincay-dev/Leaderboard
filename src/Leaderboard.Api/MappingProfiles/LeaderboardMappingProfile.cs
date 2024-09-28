using AutoMapper;
using Leaderboard.Api.Models.DTOs;
using Leaderboard.Core.Entities;

namespace Leaderboard.Api.MappingProfiles
{
  public class LeaderboardMappingProfile : Profile
  {
    public LeaderboardMappingProfile()
    {
      CreateMap<User, UserDTO>();

      CreateMap<Score, ScoreDTO>()
        .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
    }
  }
}
