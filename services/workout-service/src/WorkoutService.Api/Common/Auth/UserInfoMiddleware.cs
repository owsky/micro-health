using System.Security.Claims;
using System.Text;
using System.Text.Json;
using WorkoutService.Common.Exceptions;

namespace WorkoutService.Common.Auth;

/// <summary>
/// Middleware which extracts the username from the X-Userinfo header set by the auth server
/// </summary>
public sealed class UserInfoMiddleware(ILogger<UserInfoMiddleware> logger) : IMiddleware
{
  public async Task InvokeAsync(HttpContext context, RequestDelegate next)
  {
    if (context.Request.Path.StartsWithSegments("/api"))
    {
      if (context.Request.Headers.TryGetValue("X-Userinfo", out var headerValue))
      {
        try
        {
          var json = Encoding.UTF8.GetString(Convert.FromBase64String(headerValue.ToString()));
          var root = JsonSerializer.Deserialize<JsonElement>(json);

          var claims = new List<Claim>();

          if (root.TryGetProperty("preferred_username", out var username))
            claims.Add(new Claim(ClaimTypes.Name, username.GetString()!));
          else
            throw new ForbiddenException("Missing preferred_username field from X-Userinfo request header");

          context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Gateway"));
        }
        catch (Exception ex)
        {
          logger.LogCritical(ex, "Failed to parse X-Userinfo header.");
          throw;
        }
      }
      else
      {
        throw new ForbiddenException("Missing X-Userinfo request header");
      }

      await next(context);
    }

    await next(context);
  }
}
