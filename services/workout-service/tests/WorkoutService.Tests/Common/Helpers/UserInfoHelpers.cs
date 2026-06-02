using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using WorkoutService.Common.Auth;

namespace WorkoutService.Tests.Common.Helpers;

public static class UserInfoHelpers
{
  public static UserInfo CreateUserInfo(string username)
  {
    var claims = new[] { new Claim(ClaimTypes.Name, username) };
    var identity = new ClaimsIdentity(claims, authenticationType: "Test");
    var principal = new ClaimsPrincipal(identity);

    var httpContext = new DefaultHttpContext { User = principal };

    var accessor = Substitute.For<IHttpContextAccessor>();
    accessor.HttpContext.Returns(httpContext);

    return new UserInfo(accessor);
  }
}
