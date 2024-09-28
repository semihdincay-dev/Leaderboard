using Leaderboard.Core.Interfaces;
using Leaderboard.Core.Services;
using Leaderboard.Infrastructure.Data;
using Leaderboard.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
      ValidateIssuer = false,
      ValidateAudience = false,
    };
  });

builder.Services.AddDbContext<LeaderboardContext>(options =>
  options.UseNpgsql(builder.Configuration.GetConnectionString("LeaderboardDBConnectionString")));
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration["Redis:Connection"]));
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<IScoreService, ScoreService>();

builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Leaderboard API v1");
    c.RoutePrefix = string.Empty;
  });
}

//app.UseEndpoints(endpoints =>
//{
//  endpoints.MapControllerRoute(
//    name: "default",
//    pattern:"{controller=Home}/{action=Index}/{id?}");
//});

app.UseMiddleware<Leaderboard.Api.Middleware.ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapSwagger();
app.Run();

