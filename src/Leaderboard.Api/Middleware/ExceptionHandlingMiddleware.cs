using System.Net;
using System.Text.Json;

namespace Leaderboard.Api.Middleware
{
  public class ExceptionHandlingMiddleware
  {
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(context, ex);
      }
    }

    public Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      var response = context.Response;
      response.ContentType = "application/json";

      response.StatusCode = exception switch
      {
        KeyNotFoundException => (int)HttpStatusCode.NotFound,
        UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
        _ => (int)HttpStatusCode.InternalServerError,
      };

      var result = JsonSerializer.Serialize(new { message = exception?.Message });
      return response.WriteAsync(result);
    }
  }
}
