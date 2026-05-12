using System.Security.Claims;

namespace WorkoutService.Common.Auth;

/// <summary>
/// Class which holds the information of the user who initiated the request
/// </summary>
public sealed class UserInfo
{
  public string Username { get; }

  public UserInfo(IHttpContextAccessor httpContextAccessor)
  {
    var user = httpContextAccessor.HttpContext?.User ?? throw new InvalidOperationException("No active HTTP context.");

    Username =
      user.FindFirstValue(ClaimTypes.Name)
      ?? throw new InvalidOperationException("Claim 'preferred_username' is missing.");
  }
}
