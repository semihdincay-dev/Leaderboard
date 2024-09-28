using Leaderboard.Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Leaderboard.Core.Services
{
  public class RedisCacheService : ICacheService
  {
    private readonly IConnectionMultiplexer _redis;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
      _redis = redis;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
      var db = _redis.GetDatabase();
      var jsonData = JsonSerializer.Serialize(value);
      await db.StringSetAsync(key, jsonData, expiration);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
      var db = _redis.GetDatabase();
      var jsonData = await db.StringGetAsync(key);

      if (jsonData.IsNullOrEmpty)
        return default;

      return JsonSerializer.Deserialize<T>(jsonData);
    }
  }
}
